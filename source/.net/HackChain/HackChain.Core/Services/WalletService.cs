using HackChain.Core.Extensions;
using HackChain.Core.Infrastructure;
using HackChain.Core.Interfaces;
using HackChain.Core.Model;
using HackChain.Utilities;
using Org.BouncyCastle.Crypto.Parameters;

namespace HackChain.Core.Services
{
    public class WalletService : IWalletService
    {
        private IAccountService _accountService;

        private ECPrivateKeyParameters _privateKey;
        private ECPrivateKeyParameters PrivateKey
        {
            get
            {
                if( _privateKey == null )
                {
                    throw new HackChainException($"Please provide PrivateKey by calling '{nameof(SetPrivateKey)}'.",
                    HackChainErrorCode.Wallet_PrivateKey_Missing);
                }
                return _privateKey;
            }
        }

        public WalletService(
            IAccountService accountService
            )
        {
            _accountService = accountService;
        }
        public async Task<Transaction> GenerateTransaction(string recipientAddress, long value, long fee)
        {
            var recipient = CryptoUtilities.PublicKeyFromHex(recipientAddress);
            if (recipient == null)
            {
                throw new HackChainException($"Invalid Recipient[Address='{recipientAddress}'].",
                    HackChainErrorCode.Wallet_Invalid_Recipient);
            }

            var publickKey = CryptoUtilities.PublicKeyFromPrivateKey(PrivateKey);
            var senderAddress = CryptoUtilities.PublicKeyToHex(publickKey);

            var senderAccount = await _accountService.GetAccountByAddress(senderAddress);

            var transaction = new Transaction()
            {
                Sender = senderAddress,
                Recipient = recipientAddress,
                Nonce = senderAccount.Nonce + 1,
                Value = value,
                Fee = fee
            };

            transaction.Hash = transaction.CalculateHash();
            transaction.Signature = transaction.Sign(PrivateKey);

            return transaction;
        }

        public void SetPrivateKey(string privateKeyHex)
        {
            _privateKey = CryptoUtilities.PrivateKeyFromPrivateKeyHex(privateKeyHex);
        }
    }
}
