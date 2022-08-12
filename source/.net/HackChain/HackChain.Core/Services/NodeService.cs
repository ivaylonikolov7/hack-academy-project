using HackChain.Core.Data;
using HackChain.Core.Interfaces;
using HackChain.Core.Model;
using Microsoft.EntityFrameworkCore;
using HackChain.Core.Extensions;
using HackChain.Utilities;
using HackChain.Core.Infrastructure;
using HackChain.Node.DTO;
using AutoMapper;

namespace HackChain.Core.Services
{
    public class NodeService: INodeService
    {
        private static Dictionary<string, INodeConnector> _nodeConnectorsByUrl = new Dictionary<string, INodeConnector>();
        private static Dictionary<string, PeerNode> _peers = new Dictionary<string, PeerNode>();
        private static bool _isMining = false;
        private static NodeStatusType _nodeStatusType;
        private const string BlockNoncePlaceholder = "BlockNoncePlaceholder";
        private HackChainDbContext _db;
        private IMapper _mapper;
        private IAccountService _accountService;
        private ITransactionService _transactionService;
        private HackChainSettings _settings;
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

            return lastBlock;
        }

        private Block GenerateGenesisBlock()
        {
            var genesisBlock = new Block
            {
                Index = 1,
                Difficulty =5,
                PreviousBlockHash = "none",
                Timestamp = 1660293963,
                Nonce = 476843,
                CurrentBlockHash =  "00000f246a37ca30fa35b4e2bf62439c1b88f8d609a1a3244c8332c69614dd87",
                Data = new List<Transaction> { 
                    new Transaction { 
                        Sender = "",
                        Recipient = "04fbff67f613b7854c63e5b06eee5e880fd2ff618558e97a1cce73778579a94050c0381b973f34c192cba1662459e00902c2fbd5efd358844f964a94c39f69b91b",
                        Nonce = 1,
                        Data = null,
                        Value = 100,
                        Fee = 0,
                        Hash = "8dacfd6cf7021fea26d1e3c482db3219ab03e78124ef96e31d32fc7cdded6172",
                        Signature = "Coinbase"
                    }
                }
            };

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
            await TryAddGenesisBlock();
            _nodeStatusType = NodeStatusType.Syncing;

            // connect to all known nodes and their peers
            await ConnectToPeerNodes();

            // get the longest chain - pick 1 node
            // sync the missing blocks
            
            //perform sync
            await TryAddBlock(_nodeWithLongestChain.LastBlockIndex, _nodeWithLongestChain.BaseUrl);
                
            _nodeStatusType = NodeStatusType.Synced;

            return;
        }

        private async Task TryAddGenesisBlock()
        {
            var lastBlock = await GetLastBlock();
            if (lastBlock == null)
            {
                var genesisBlock = GenerateGenesisBlock();
                _db.Blocks.Add(genesisBlock);
                await _db.SaveChangesAsync();
                await UpdateAccounts(genesisBlock);
                await _db.SaveChangesAsync();
            }
        }

        private async Task RefreshPendingTransactions(string peerNodeUrl)
        {
            var nodeConnector = GetNodeConnector(peerNodeUrl);
            
            var pendingTransactions = await nodeConnector.GetPendingTransactions();

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

        private INodeConnector GetNodeConnector(string nodeUrl)
        {
            if(_nodeConnectorsByUrl.ContainsKey(nodeUrl) == false)
            {
                _nodeConnectorsByUrl[nodeUrl] = new NodeConnector(nodeUrl);
            }

            return _nodeConnectorsByUrl[nodeUrl];
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
            var nodeConnector = GetNodeConnector(peerNodeUrl);

            PeerNode thisNode = new PeerNode
            {
                BaseUrl = _settings.BaseUrl,
                Id = _settings.NodeId
            };
            var thisNodeDTO = _mapper.Map<PeerNodeDTO>(thisNode);
            var addPeerNodeResponse = await nodeConnector.AddPeerNode(thisNodeDTO);

            return addPeerNodeResponse;
        }

        private async Task<List<string>> TraverseAllNodes(IEnumerable<string> knownPeerNodesBaseUrls)
        {
            var nodeUrls = knownPeerNodesBaseUrls.ToDictionary(x => x, x => false);

            string? url = nodeUrls
                .Where(kv => kv.Value == false)
                .Select(kv => kv.Key)
                .FirstOrDefault();

            while(url != null)
            {
                var nodeConnector = GetNodeConnector(url);

                var nodePeers = await nodeConnector.GetPeerNodes();
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
                // fetch next non traversed url
                url = nodeUrls
                    .Where(kv => kv.Value == false)
                    .Select(kv => kv.Key)
                    .FirstOrDefault();
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
            var nodeConnector = GetNodeConnector(peerNodeUrl);

            var nodeStatus = await nodeConnector.GetNodeStatus();

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
            var nodeConnector = GetNodeConnector(peerNodeUrl);

            var lastLocalBlock = await GetLastBlock();
            if(lastLocalBlock != null
                && lastLocalBlock.Index < remoteBlockIndex)
            {
                // longer chain found
                // find common block
                var lastCommonLocalBlockIndex = await GetLastCommonLocalBlock(lastLocalBlock, nodeConnector);

                if (lastCommonLocalBlockIndex < lastLocalBlock.Index)
                {
                    // revert accounts present in transactions in blocks till the last common block
                    //_accountService.RevertTransactionData()
                    // delete transactions in blocks till the last common block?
                    // we need to delete transactions, because the assumtion is that only valid transactions are in the mem pool - not the case when reverting mulitple blocks
                }

                long nextBlockIndexForProcessing = lastCommonLocalBlockIndex++;
                while(nextBlockIndexForProcessing < remoteBlockIndex)
                {
                    // process blocks one by one
                }

                // get pending transactions from the node with the longest chain after syncing
                await RefreshPendingTransactions(peerNodeUrl);
            }

            
        }

        private async Task<long> GetLastCommonLocalBlock(Block currentLocalBlock, INodeConnector nodeConnector)
        {
            //TODO: cache fetched remote blocks for further processing
            var remoteBlockDTO = await nodeConnector.GetBlockByIndex(currentLocalBlock.Index);
            var remoteBlock = _mapper.Map<Block>(remoteBlockDTO);

            //TODO: validate missing blocks, with index larger than the current local block
            // perform simple block validation - difficulty and hash
            remoteBlock.Validate(_settings.Difficulty);

            while(currentLocalBlock.CurrentBlockHash != remoteBlock.CurrentBlockHash)
            {
                long newIndex = currentLocalBlock.Index--;
                currentLocalBlock = await GetBlockByIndex(newIndex);
                remoteBlockDTO = await nodeConnector.GetBlockByIndex(newIndex);
                remoteBlock = _mapper.Map<Block>(remoteBlockDTO);
                remoteBlock.Validate(_settings.Difficulty);
            }

            return currentLocalBlock.Index;
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
