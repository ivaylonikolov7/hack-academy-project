using HackChain.Core.Data;
using HackChain.Core.Interfaces;
using HackChain.Core.Model;
using Microsoft.EntityFrameworkCore;
using HackChain.Core.Extensions;
using HackChain.Utilities;
using HackChain.Core.Infrastructure;
using HackChain.Node.DTO;
using AutoMapper;
using HackChain.Core.Extensions;

namespace HackChain.Core.Services
{
    public class NodeService: INodeService
    {
        private static Dictionary<string, PeerNode> _peers = new Dictionary<string, PeerNode>();
        private static bool _isMining = false;
        private static NodeStatusType _nodeStatusType;
        private const string BlockNoncePlaceholder = "BlockNoncePlaceholder";
        private HackChainDbContext _db;
        private IMapper _mapper;
        private IAccountService _accountService;
        private ITransactionService _transactionService;
        private HackChainSettings _settings;
        private NodeConnector _nodeConnector;
        private NodeStatusDTO _nodeWithLongestChain;

        public NodeService(
            HackChainDbContext db,
            IAccountService accountService,
            ITransactionService transactionService,
            IMapper mapper,
            HackChainSettings settings
            )
        {
            _db = db;
            _mapper = mapper;
            _accountService = accountService;
            _transactionService = transactionService;
            _settings = settings;
            _nodeConnector = new NodeConnector();
        }

        public async Task<Block> GetBlockByIndex(long index)
        {
            var block = await _db.Blocks
                .Include(b => b.Data)
                .FirstOrDefaultAsync(a => a.Index == index);

            return block;
        }

        public async Task<Block> MineBlock()
        {
            if(_isMining)
            {
                return await GetLastBlock(); ;
            }

            _isMining = true;
            try
            {
                var transactions = await _db.Transactions
                    .Where(tr => tr.BlockId == null)
                    .OrderByDescending(tr => tr.Fee)
                    .Take(10)
                    .ToListAsync();

                var lastBlock = await GetLastBlock();

                var currentBlock = new Block
                {
                    Index = lastBlock.Index + 1,
                    Difficulty = _settings.Difficulty,
                    PreviousBlockHash = lastBlock.CurrentBlockHash,
                    Timestamp = DateTime.UtcNow.ToUnixTime()
                };

                AddCoinbaseTransaction(transactions, currentBlock.Index);
                currentBlock.AddTransactions(transactions);
                CalculateBlockHash(currentBlock);

                _db.Blocks.Add(currentBlock);
                await _db.SaveChangesAsync();

                await UpdateAccounts(currentBlock);
                await _db.SaveChangesAsync();
                return currentBlock;
            }
            finally
            {
                _isMining = false;
            }
            
            return null;
        }

        private async Task UpdateAccounts(Block currentBlock)
        {
            //TODO: optimaze db operations - read all account in one query
            foreach (var tr in currentBlock.Data)
            {
                await _accountService.ApplyTransactionData(tr);
            }
        }

        private async Task<Block> GetLastBlock()
        {
            var lastBlock = await _db.Blocks
                .OrderByDescending(b => b.Index)
                .FirstOrDefaultAsync();

            if(lastBlock == null)
            {
                var genesisBlock = GenerateGenesisBlock();
                _db.Blocks.Add(genesisBlock);
                await _db.SaveChangesAsync();
                await UpdateAccounts(genesisBlock);
                await _db.SaveChangesAsync();

                lastBlock = genesisBlock;
            }

            return lastBlock;
        }

        private Block GenerateGenesisBlock()
        {
            var genesisBlock = new Block
            {
                Index = 1,
                Difficulty = _settings.Difficulty,
                PreviousBlockHash = "none",
                Timestamp = DateTime.UtcNow.ToUnixTime()
            };

            var transactions = new List<Transaction>();
            AddCoinbaseTransaction(transactions, genesisBlock.Index);
            genesisBlock.AddTransactions(transactions);

            CalculateBlockHash(genesisBlock);

            return genesisBlock;
        }

        private void AddCoinbaseTransaction(List<Transaction> transactions, long blockIndex)
        {
            long totalFees = transactions.Sum(tr => tr.Fee);

            var coinbaseTransaction = Transaction.Coinbase(_settings.MinerAddress, blockIndex, _settings.CoinbaseValue + totalFees);
            transactions.Add(coinbaseTransaction);
        }


        private void CalculateBlockHash(Block block)
        {
            var blockForHashing = block.SerializeForMining(BlockNoncePlaceholder);
            string hash = string.Empty;
            string leadingZeroes = new string('0', (int)block.Difficulty);
            long nonce = 0;
            do
            {
                hash = CryptoUtilities.CalculateSHA256Hex(blockForHashing.Replace(BlockNoncePlaceholder, nonce.ToString()));
                nonce++;
            }
            while (hash.StartsWith(leadingZeroes) == false);

            block.Nonce = nonce;
            block.CurrentBlockHash = hash;
        }

        public async Task<NodeStatus> GetNodeStatus()
        {
            var lastBlock = await _db.Blocks.OrderByDescending(b => b.Index).FirstOrDefaultAsync();

            var status = new NodeStatus()
            {
                NodeId = _settings.NodeId,
                BaseUrl = _settings.BaseUrl,
                LastBlockIndex = lastBlock?.Index ?? 0,
                LastBlockHash = lastBlock?.CurrentBlockHash ?? string.Empty,
                LastBlockTimestamp = lastBlock?.Timestamp ?? 0,
                NonZeroAddressesCount = await _db.Accounts.CountAsync(a => a.Balance != 0),
                ProcessedTransactionsCount = await _db.Transactions.CountAsync(t => t.BlockId != null),
                PendingTransactionsCount = await _db.Transactions.CountAsync(t => t.BlockId == null),
                IsMining = _isMining,
                Status = _nodeStatusType
            };

            return status;
        }

        public async Task Init()
        {
            _nodeStatusType = NodeStatusType.Syncing;
            // connect to all known nodes and their peers
            await ConnectToPeerNodes();

            // get the longest chain - pick 1 node
            // sync the missing blocks
            
            //perform sync
            await TryAddBlock(_nodeWithLongestChain.LastBlockIndex, _nodeWithLongestChain.BaseUrl);
                
            // get pending transactions from the node with the longest chain after syncing
            await RefreshPendingTransactions();

            _nodeStatusType = NodeStatusType.Synced;

            return;
        }

        private async Task RefreshPendingTransactions()
        {
            _nodeConnector.SetBaserUrl(_nodeWithLongestChain.BaseUrl);
            var pendingTransactions = await _nodeConnector.GetPendingTransactions();

            var domainPendingTransactions = _mapper.Map<IEnumerable<Transaction>>(pendingTransactions);
            foreach (var tr in domainPendingTransactions)
            {
                try
                {
                    await _transactionService.AddTransaction(tr);
                }
                catch (Exception ex)
                {
                }
            }
        }

        private async Task ConnectToPeerNodes()
        {
            List<string> allnodesUrls = await TraverseAllNodes(_settings.KnownPeerNodesBaseUrls);

            foreach (var url in allnodesUrls)
            {
                await TryRegisterToPeerNode(url);
                
                await TryAddPeerNode(url);
            }
        }

        public async Task<bool> TryRegisterToPeerNode(string peerNodeUrl)
        {
            _nodeConnector.SetBaserUrl(peerNodeUrl);
            PeerNode thisNode = new PeerNode
            {
                BaseUrl = _settings.BaseUrl,
                Id = _settings.NodeId
            };
            var thisNodeDTO = _mapper.Map<PeerNodeDTO>(thisNode);
            var addPeerNodeResponse = await _nodeConnector.AddPeerNode(thisNodeDTO);

            return addPeerNodeResponse;
        }

        private async Task<List<string>> TraverseAllNodes(IEnumerable<string> knownPeerNodesBaseUrls)
        {
            var nodeUrls = knownPeerNodesBaseUrls.ToDictionary(x => x, x => false);

            string url = nodeUrls
                .Where(kv => kv.Value == false)
                .Select(kv => kv.Key)
                .FirstOrDefault();

            while(url != null)
            {
                _nodeConnector.SetBaserUrl(url);
                var nodePeers = await _nodeConnector.GetPeerNodes();
                foreach (var peer in nodePeers)
                {
                    if(nodeUrls.ContainsKey(peer.BaseUrl) == false)
                    {
                        // add the url for trversing
                        nodeUrls[peer.BaseUrl] = false;
                    }
                }

                // marking url as traversed
                nodeUrls[url] = true;
            }

            return nodeUrls
                .Select(kv => kv.Key)
                .ToList();
        }

        public Task<IEnumerable<PeerNode>> GetPeerNodes()
        {
            return Task.FromResult(_peers.Select(p => p.Value));
        }

        public async Task<bool> TryAddPeerNode(string peerNodeUrl)
        {
            _nodeConnector.SetBaserUrl(peerNodeUrl);
            var nodeStatus = await _nodeConnector.GetNodeStatus();

            if(_nodeWithLongestChain == null
                || _nodeWithLongestChain.LastBlockIndex < nodeStatus.LastBlockIndex)
            {
                _nodeWithLongestChain = nodeStatus;
            }

            var peerNodeCandidate = new PeerNode
            {
                BaseUrl = nodeStatus.BaseUrl,
                Id = nodeStatus.NodeId,
                LastUpdatedOn = DateTime.UtcNow,
                Rating = 100
            };

            var existingPeer = _peers[peerNodeCandidate.Id];
            if (existingPeer.BaseUrl != peerNodeCandidate.BaseUrl)
            {
                //throw? can a node change its base url
                //how to prevent nodes to spoof peers?
                //how to enable nodes to claim their identity after some other node has taken their id?
            }

            var peersWithSameUrl = _peers
                .Where(p => p.Value.BaseUrl == peerNodeCandidate.BaseUrl)
                .Select(p => p)
                .ToList();

            if (peersWithSameUrl.Any())
            {
                //throw? it makes no sense 2 nodes with different Id to have the same url
                //it is not possible to figth against multiple urls pointing to the same Node
            }

            if (_peers.ContainsKey(peerNodeCandidate.Id) == false)
            {
                _peers[peerNodeCandidate.Id] = peerNodeCandidate;
            }

            return true;
        }

        public async Task TryAddBlock(long remoteBlockIndex, string peerNodeUrl)
        {
            _nodeConnector.SetBaserUrl(peerNodeUrl);
            
            var lastLocalBlock = await GetLastBlock();
            if(lastLocalBlock != null
                && lastLocalBlock.Index < remoteBlockIndex)
            {
                // find common block
                var lastCommonLocalBlock = await GetLastCommonLocalBlock(lastLocalBlock);
                // revert transactions in blocks till the last common block - in memory

            }
        }

        private async Task<long> GetLastCommonLocalBlock(Block currentLocalBlock)
        {
            //TODO: cache fetched remote blocks for further processing
            var remoteBlockDTO = await _nodeConnector.GetBlockByIndex(currentLocalBlock.Index);
            var remoteBlock = _mapper.Map<Block>(remoteBlockDTO);

            // perform simple block validation - difficulty and hash
            remoteBlock.Validate(_settings.Difficulty);

            while(currentLocalBlock.CurrentBlockHash != remoteBlock.CurrentBlockHash)
            {
                long newIndex = currentLocalBlock.Index--;
                currentLocalBlock = await GetBlockByIndex(newIndex);
                remoteBlockDTO = await _nodeConnector.GetBlockByIndex(newIndex);
            }

            return remoteBlockDTO.Index;
        }

        public void PropagateTransaction(Transaction transaction)
        {
            throw new NotImplementedException();
        }

        public void PropagateBlock(Block block)
        {
            throw new NotImplementedException();
        }
    }
}
