using HackChain.Core.Data;
using HackChain.Core.Extensions;
using HackChain.Core.Interfaces;
using HackChain.Core.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackChain.Core.Services
{
    public class AccountService : IAccountService
    {
        private HackChainDbContext _db;

        public AccountService(HackChainDbContext db)
        {
            _db = db;
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

        public Task RevertTransactionData(Transaction transaction)
        {
            throw new NotImplementedException();
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
