using HackChain.Core.Model;

namespace HackChain.Core.Interfaces
{
    public interface IAccountService
    {
        Task<Account> GetAccountByAddress(string address);
        Task ApplyTransactionData(Transaction transaction);
        Task RevertTransactionData(Transaction transaction);
    }
}
