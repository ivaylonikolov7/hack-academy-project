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
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            var forHashing = transaction.SerializeForHashing();
            var hash = CryptoUtilities.CalculateSHA256(forHashing);

            return hash;
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

        public static bool Verify(this Transaction transaction)
        {
            var publicKey = CryptoUtilities.PublicKeyFromBase58(transaction.Sender);

            var hash = transaction.CalculateHash();
            var isValid = CryptoUtilities.VerifySignature(publicKey, transaction.Signature, hash);

            return isValid;
        }
    }
}
