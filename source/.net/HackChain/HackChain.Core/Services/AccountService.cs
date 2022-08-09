using HackChain.Core.Data;
using HackChain.Core.Extensions;
using HackChain.Core.Interfaces;
using HackChain.Core.Model;
using Microsoft.EntityFrameworkCore;

namespace HackChain.Core.Services
{
    public class AccountService : IAccountService
    {
        private HackChainDbContext _db;

        public AccountService(HackChainDbContext db)
        {
            _db = db;
        }
        public async Task<Account> GetAccountByAddress(string address)
        {
            var account = await _db.Accounts.FirstOrDefaultAsync(a => a.Address == address);

            return account;
        }

        public async Task ApplyTransactionData(Transaction transaction)
        {
            if (transaction.IsCoinbase() == false)
            {
                long spentAmount = transaction.Value + transaction.Fee;
                var senderAccount = await GetAccount(transaction.Sender);
                senderAccount.Balance -= spentAmount;
                senderAccount.Nonce += 1;
            }

            var recipientAccount = await GetAccount(transaction.Recipient);
            recipientAccount.Balance += transaction.Value;
        }

        public async Task RevertTransactionData(Transaction transaction)
        {
            if (transaction.IsCoinbase() == false)
            {
                long spentAmount = transaction.Value + transaction.Fee;
                var senderAccount = await GetAccount(transaction.Sender);
                senderAccount.Balance += spentAmount;
                senderAccount.Nonce -= 1;
            }

            var recipientAccount = await GetAccount(transaction.Recipient);
            recipientAccount.Balance -= transaction.Value;
        }

        private async Task<Account> GetAccount(string address)
        {
            var existingAccount = await _db.Accounts
                .FirstOrDefaultAsync(a => a.Address == address);
            if(existingAccount == null)
            {
                var newAccount = new Account
                {
                    Address = address
                };

                _db.Accounts.Add(newAccount);
                existingAccount = newAccount;
            }

            return existingAccount;
        }
    }
}
