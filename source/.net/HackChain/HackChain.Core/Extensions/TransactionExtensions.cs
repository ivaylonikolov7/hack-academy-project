using HackChain.Core.Infrastructure;
using HackChain.Core.Model;
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
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            var hash = transaction.CalculateHash();
            var signature = CryptoUtilities.SignData(hash, privateKey);

            return signature;
        }

        public static bool VerifySignature(this Transaction transaction)
        {
            var publicKey = CryptoUtilities.PublicKeyFromHex(transaction.Sender);

            var hash = transaction.CalculateHash();
            var isValid = CryptoUtilities.VerifySignature(publicKey, transaction.Signature, hash);

            return isValid;
        }

        public static void Validate(this Transaction transaction)
        {
            // sender / recipient can be parsed to public keys
            var sender = CryptoUtilities.PublicKeyFromHex(transaction.Sender);
            if(sender == null)
            {
                throw new HackChainException($"Transaction[Hash='{transaction.Hash}'] has invalid Sender[Address='{transaction.Sender}'].",
                    HackChainErrorCode.Transaction_Invalid_Sender);
            }

            var recipient = CryptoUtilities.PublicKeyFromHex(transaction.Recipient);
            if (recipient == null)
            {
                throw new HackChainException($"Transaction[Hash='{transaction.Hash}'] has invalid Recipient[Address='{transaction.Sender}'].",
                    HackChainErrorCode.Transaction_Invalid_Recipient);
            }
            // value > 0 - what about overflow? What are BigInteger's limitations? memory?
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
            // check signiture is valid
            
            if (transaction.VerifySignature() == false)
            {
                throw new HackChainException($"Transaction[Hash='{transaction.Hash}'] has invalid Signature='{transaction.Signature}'.",
                    HackChainErrorCode.Transaction_Invalid_Signature);
            }
        }
    }
}
