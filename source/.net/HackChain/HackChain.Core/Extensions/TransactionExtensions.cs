using HackChain.Core.Infrastructure;
using HackChain.Core.Model;
using HackChain.Node.DTO;
using HackChain.Utilities;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Parameters;

namespace HackChain.Core.Extensions
{
    public static class TransactionExtensions
    {
        public static string SerializeForHashing(this Transaction tr)
        {
            var result = $"{{\"Sender\":\"{tr.Sender}\",\"Recipient\":\"{tr.Recipient}\",\"Nonce\":{tr.Nonce},\"Value\":{tr.Value},\"Fee\":{tr.Fee}}}";

            return result;
        }
        public static string SerializeForBlockHashing(this Transaction tr)
        {
            var result = $"{{\"Sender\":\"{tr.Sender}\",\"Recipient\":\"{tr.Recipient}\",\"Nonce\":{tr.Nonce},\"Data\":\"{tr.Data}\",\"Value\":{tr.Value},\"Fee\":{tr.Fee},\"Hash\":\"{tr.Hash}\",\"Signature\":\"{tr.Signature}\"}}";

            return result;
        }
        public static string Serialize(this Transaction transaction, Formatting formating = Formatting.Indented)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            var result = JsonConvert.SerializeObject(transaction, formating, settings);

            return result;
        }
        public static string CalculateHash(this Transaction transaction)
        {
            var forHashing = transaction.SerializeForHashing();
            var hashHex = CryptoUtilities.CalculateSHA256Hex(forHashing);

            return hashHex;
        }
        public static string Sign(this Transaction transaction, ECPrivateKeyParameters privateKey)
        {
            var forSigning = transaction.SerializeForHashing();
            var signature = CryptoUtilities.SignDataDeterministicly(forSigning, privateKey);

            return signature;
        }

        public static bool VerifySignature(this Transaction transaction)
        {
            var publicKey = CryptoUtilities.PublicKeyFromHex(transaction.Sender);

            var forVerifying = transaction.SerializeForHashing();
            var isValid = CryptoUtilities.VerifySignature(publicKey, transaction.Signature, forVerifying);

            return isValid;
        }

        public static void Validate(this Transaction transaction, Account senderAccount)
        {
            if (transaction.IsCoinbase() == false)
            {
                // sender / recipient can be parsed to public keys
                var sender = CryptoUtilities.PublicKeyFromHex(transaction.Sender);
                if (sender == null)
                {
                    throw new HackChainException($"Transaction[Hash='{transaction.Hash}'] has invalid Sender[Address='{transaction.Sender}'].",
                        HackChainErrorCode.Transaction_Invalid_Sender);
                }

                // check signiture is valid
                if (transaction.VerifySignature() == false)
                {
                    throw new HackChainException($"Transaction[Hash='{transaction.Hash}'] has invalid Signature='{transaction.Signature}'.",
                        HackChainErrorCode.Transaction_Invalid_Signature);
                }

                long nextValidNonce = (senderAccount?.Nonce ?? 0) + 1;
                bool hasValidNonce = transaction.Nonce == nextValidNonce;
                // nonse should be precisely the next one
                if (hasValidNonce == false)
                {
                    throw new HackChainException($"Transaction[Hash='{transaction.Hash}'] has invalid Nonce='{transaction.Nonce}'. The next valid Nonce is '{nextValidNonce}'.",
                        HackChainErrorCode.Transaction_Invalid_Nonce);
                }

                long availableBallance = senderAccount?.Balance ?? 0;
                long neededBalance = transaction.Value + transaction.Fee;
                bool hasEnoughFunds = availableBallance >= neededBalance;
                // account should have enough funds
                if (hasEnoughFunds == false)
                {
                    throw new HackChainException($"Transaction[Hash='{transaction.Hash}'] Value+Fee='{neededBalance}' is greater then the available balance='{availableBallance}'.",
                        HackChainErrorCode.Transaction_Insufficient_Balance);
                }
            }
            

            var recipient = CryptoUtilities.PublicKeyFromHex(transaction.Recipient);
            if (recipient == null)
            {
                throw new HackChainException($"Transaction[Hash='{transaction.Hash}'] has invalid Recipient[Address='{transaction.Sender}'].",
                    HackChainErrorCode.Transaction_Invalid_Recipient);
            }
            // value > 0 - what about overflow?
            if (transaction.Value < 0)
            {
                throw new HackChainException($"Transaction[Hash='{transaction.Hash}'] has invalid Value='{transaction.Sender}'. Value cannot be less than 0.",
                    HackChainErrorCode.Transaction_Invalid_Value);
            }
            // fee > 0 - what about overflow?
            if (transaction.Fee < 0)
            {
                throw new HackChainException($"Transaction[Hash='{transaction.Hash}'] has invalid Fee='{transaction.Sender}'. Fee cannot be less than 0.",
                    HackChainErrorCode.Transaction_Invalid_Fee);
            }
            // check hash is correct
            var calculatedHash = transaction.CalculateHash();
            if (transaction.Hash != calculatedHash)
            {
                throw new HackChainException($"Transaction Hash missmatch. Provided Hash('{transaction.Hash}') doesn't match calculated Hash('{calculatedHash}').",
                    HackChainErrorCode.Transaction_Invalid_Hash);
            }           
        }

        public static bool IsCoinbase(this Transaction transaction)
        {
            return transaction.Signature == "Coinbase";
        }
    }
}
