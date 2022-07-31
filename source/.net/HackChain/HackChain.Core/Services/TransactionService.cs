using HackChain.Core.Data;
using HackChain.Core.Extensions;
using HackChain.Core.Infrastructure;
using HackChain.Core.Interfaces;
using HackChain.Core.Model;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace HackChain.Core.Services
{
    public class TransactionService : ITransactionService
    {
        private HackChainDbContext _db;

        public TransactionService(HackChainDbContext db)
        {
            _db = db;
        }
        public async Task AddTransaction(Transaction transaction)
        {
            transaction.Validate();
            var existingTransaction = await _db.Transactions.FirstOrDefaultAsync(tr => tr.Hash == transaction.Hash);
            if (existingTransaction != null)
            {
                throw new HackChainException($"Transaction[Hash='{transaction.Hash}'] is duplicated.",
                    HackChainErrorCode.Transaction_Duplicate);
            }

            transaction.IsValidForNextBlock = CalculateIsValidForNextBlock(transaction);

            _db.Transactions.Add(transaction);

            await _db.SaveChangesAsync();
        }

        public async Task<Transaction> GetTransactionByHash(string hash)
        {
            var transaction = await _db.Transactions.FirstOrDefaultAsync(a => a.Hash == hash);

            return transaction;
        }

        public async Task<IEnumerable<Transaction>> GetPendingTransactions()
        {
            var transactions = await
                _db.Transactions
                    .Where(t => t.BlockIndex == null)
                    .ToListAsync();

            return transactions;
        }

        private bool CalculateIsValidForNextBlock(Transaction transaction)
        {
            var account = _db.Accounts.FirstOrDefault(a => a.Address == transaction.Sender);
            long nextValidNonce = (account?.Nonce ?? 0) + 1;
            bool hasValidNonce = transaction.Nonce == nextValidNonce;

            decimal availableBallance = account?.Balance ?? 0;
            bool hasEnoughFunds = availableBallance >= transaction.Value + transaction.Fee;

            return hasValidNonce && hasEnoughFunds;
        }
    }
}
