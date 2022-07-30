using HackChain.Core.Model;

namespace HackChain.Core.Interfaces
{
    public interface INodeService
    {
        Task AddTransaction(Transaction transaction);
        Task<Transaction> GetTransactionByHash(string hash);
        Task<IEnumerable<Transaction>> GetPendingTransactions();

        Task<Block> GetBlockByIndex(long index);
        Task MineBlock();

        Task<Account> GetAccountByAddress(string address);
    }
}
