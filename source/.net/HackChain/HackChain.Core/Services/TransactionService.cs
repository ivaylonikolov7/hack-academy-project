using HackChain.Core.Data;
using HackChain.Core.Extensions;
using HackChain.Core.Infrastructure;
using HackChain.Core.Interfaces;
using HackChain.Core.Model;
using HackChain.Node.DTO;
using HackChain.Utilities;
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

            ValidateBalanceAndNonce(transaction);

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
                    .Where(t => t.BlockId == null)
                    .ToListAsync();

            return transactions;
        }

        private void ValidateBalanceAndNonce(Transaction transaction)
        {
            var account = _db.Accounts.FirstOrDefault(a => a.Address == transaction.Sender);
            long nextValidNonce = (account?.Nonce ?? 0) + 1;
            bool hasValidNonce = transaction.Nonce == nextValidNonce;
            // nonse should be precisely the next one
            if (hasValidNonce == false)
            {
                throw new HackChainException($"Transaction[Hash='{transaction.Hash}'] has invalid Nonce='{transaction.Nonce}'. The next valid Nonce is '{nextValidNonce}'.",
                    HackChainErrorCode.Transaction_Invalid_Nonce);
            }

            long availableBallance = account?.Balance ?? 0;
            long neededBalance = transaction.Value + transaction.Fee;
            bool hasEnoughFunds = availableBallance >= neededBalance;
            // account should have enough funds
            if (hasEnoughFunds == false)
            {
                throw new HackChainException($"Transaction[Hash='{transaction.Hash}'] Value+Fee='{neededBalance}' is greater then the available balance='{availableBallance}'.",
                    HackChainErrorCode.Transaction_Insufficient_Balance);
            }

            var existingTransactionFromSameAddress = _db.Transactions.FirstOrDefault(t => t.Sender == transaction.Sender && t.Nonce == transaction.Nonce);
            if (existingTransactionFromSameAddress != null)
            {
                throw new HackChainException($"There is already pending Transaction[Hash='{existingTransactionFromSameAddress.Hash}'] from the same address='{transaction.Sender}' with the same Nonce='{transaction.Nonce}'.",
                    HackChainErrorCode.Transaction_Duplicate);
            }
        }

        public async Task<IEnumerable<Transaction>> GetTransactionByAddress(string address)
        {
            var parsedAddress = CryptoUtilities.PublicKeyFromHex(address);
            if (parsedAddress == null)
            {
                throw new HackChainException($"Address='{address}' is not valid.",
                    HackChainErrorCode.Invalid_Address);
            }
            var transactions = await _db.Transactions
                .Where(tr => tr.Sender == address || tr.Recipient == address)
                .Where(tr => tr.BlockId != null)
                .OrderBy(tr => tr.Block.Index)
                .Include(tr => tr.Block)
                .ToListAsync();

            return transactions;
        }
    }
}
