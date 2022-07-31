using HackChain.Core.Data;
using HackChain.Core.Interfaces;
using HackChain.Core.Model;
using Microsoft.EntityFrameworkCore;
using HackChain.Core.Extensions;
using HackChain.Core.Infrastructure;

namespace HackChain.Core.Services
{
    public class NodeService: INodeService
    {
        private HackChainDbContext _db;

        public NodeService(HackChainDbContext db)
        {
            _db = db;
        }

        public async Task<Account> GetAccountByAddress(string address)
        {
            var account = await _db.Accounts.FirstOrDefaultAsync(a => a.Address == address);

            return account;
        }

        public async Task<Block> GetBlockByIndex(long index)
        {
            var block = await _db.Blocks.FirstOrDefaultAsync(a => a.Index == index);

            return block;
        }

        public Task MineBlock()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Transaction>> GetPendingTransactions()
        {
            var transactions = await
                _db.Transactions
                    .Where(t => t.BlockIndex == null)
                    .ToListAsync();

            return transactions;
        }
    }
}
