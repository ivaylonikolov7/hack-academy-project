using HackChain.Core.Data;
using HackChain.Core.Interfaces;
using HackChain.Core.Model;
using Microsoft.EntityFrameworkCore;
using HackChain.Core.Extensions;
using HackChain.Utilities;

namespace HackChain.Core.Services
{
    public class NodeService: INodeService
    {
        private static Dictionary<string, PeerNode> _peers = new Dictionary<string, PeerNode>();
        private static bool _isMining = false;
        private const string BlockNoncePlaceholder = "BlockNoncePlaceholder";
        private HackChainDbContext _db;
        private IAccountService _accountService;
        private HackChainSettings _settings;

        public NodeService(
            HackChainDbContext db,
            IAccountService accountService,
            HackChainSettings settings)
        {
            _db = db;
            _accountService = accountService;
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
                Status = NodeStatusType.Synced
            };

            return status;
        }

        public Task Init()
        {
            
        }

        public Task<IEnumerable<PeerNode>> GetPeerNodes()
        {
            return Task.FromResult(_peers.Select(p => p.Value));
        }

        public void AddPeerNode(PeerNode peerNodeCandidate)
        {
            //try to connect to the node and get its peers?
            TryConnectPeerNode(peerNodeCandidate);
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

            _peers[peerNodeCandidate.Id] = peerNodeCandidate;
        }

        private void TryConnectPeerNode(PeerNode peerNodeCandidate)
        {
            throw new NotImplementedException();
        }

        public void AddBlock(Block block)
        {
            throw new NotImplementedException();
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
