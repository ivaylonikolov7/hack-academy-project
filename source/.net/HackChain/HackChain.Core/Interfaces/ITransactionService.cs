using HackChain.Core.Model;

namespace HackChain.Core.Interfaces
{
    public interface ITransactionService
    {
        Task AddTransaction(Transaction transaction);
        Task<Transaction> GetTransactionByHash(string hash);
        Task<IEnumerable<Transaction>> GetTransactionByAddress(string address);
        Task<IEnumerable<Transaction>> GetPendingTransactions();
    }
}
