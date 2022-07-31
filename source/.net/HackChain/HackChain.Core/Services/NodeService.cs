using HackChain.Core.Data;
using HackChain.Core.Interfaces;
using HackChain.Core.Model;
using Microsoft.EntityFrameworkCore;
using HackChain.Core.Extensions;
using HackChain.Core.Extensions;
using System.Text;
using HackChain.Utilities;

namespace HackChain.Core.Services
{
    public class NodeService: INodeService
    {
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

        public async Task<Account> GetAccountByAddress(string address)
        {
            var account = await _db.Accounts.FirstOrDefaultAsync(a => a.Address == address);

            return account;
        }

        public async Task<Block> GetBlockByIndex(long index)
        {
            var block = await _db.Blocks
                .Include(b => b.Data)
                .FirstOrDefaultAsync(a => a.Index == index);

            return block;
        }

        public async Task MineBlock()
        {
            var transactions = await _db.Transactions
                .Where(tr => tr.BlockIndex == null && tr.IsValidForNextBlock)
                .OrderByDescending(tr => tr.Fee)
                .Take(10)
                .ToListAsync();
            AddCoinbaseTransaction(transactions);

            var lastBlock = await GetLastBlock();

            var currentBlock = new Block
            {
                Index = lastBlock.Index + 1,
                Difficulty = _settings.Difficulty,
                PreviousBlockHash = lastBlock.CurrentBlockHash,
                Timestamp = DateTime.UtcNow.ToUnixTime()
            };

            currentBlock.AddTransactions(transactions);
            CalculateBlockHash(currentBlock);

            _db.Blocks.Add(currentBlock);
            await _db.SaveChangesAsync();

            await UpdateAccounts(currentBlock);
            await _db.SaveChangesAsync();
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
                .Take(1)
                .FirstOrDefaultAsync();

            if(lastBlock == null)
            {
                var genesisBlock = GenerateGenesisBlock();
                _db.Blocks.Add(genesisBlock);
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

            var transactions = GetGenesisBlockTransactions();
            AddCoinbaseTransaction(transactions);
            genesisBlock.AddTransactions(transactions);

            CalculateBlockHash(genesisBlock);

            return genesisBlock;
        }

        private void AddCoinbaseTransaction(List<Transaction> transactions)
        {
            decimal totalFees = transactions.Sum(tr => tr.Fee);

            var coinbaseTransaction = Transaction.Coinbase(_settings.MinerAddress, _settings.CoinbaseValue + totalFees);
            transactions.Add(coinbaseTransaction);
        }

        private List<Transaction> GetGenesisBlockTransactions()
        {
            //TODO: read json file, generate Coinbase transactions in a loop
            return new List<Transaction>()
            {
                Transaction.Coinbase(
                    "044842ce6522e4442ccf446c9d28e7be0aa26b83934d60289e3c1f9eba49e44dd6134b7b22ff8a38e86e69683843e3f058f326001ff4fee56e94a1e9681cda4bda",
                    100000)
            };
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
    }
}
