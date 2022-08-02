using HackChain.Core.Extensions;
using HackChain.Core.Model;
using HackChain.Utilities;
using Org.BouncyCastle.Crypto.Parameters;
using System.Text;

namespace HackChain.ConsoleTests
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //TestKeys();

            //TestTransaction();

            TestMinerTransaction();

            //TestBlock();
        }


        private static void TestKeys()
        {
            var keypair = CryptoUtilities.GenerateRandomKeys();
            var privateKey = (ECPrivateKeyParameters)keypair.Private;

            var privateKeyHex = CryptoUtilities.PrivateKeyToHexString(privateKey);

            var restoredPrivateKey = CryptoUtilities.PrivateKeyFromPrivateKeyHex(privateKeyHex);

            var restoredPublicKey = CryptoUtilities.PublicKeyFromPrivateKey(restoredPrivateKey);

            var publicKeyHex = CryptoUtilities.PublicKeyToHex(restoredPublicKey);

            var restoredPublicKeyFromHex = CryptoUtilities.PublicKeyFromHex(publicKeyHex);

            Console.WriteLine(restoredPublicKeyFromHex.Equals(restoredPublicKey));


            Console.WriteLine(
$@"Private key (decimal): '{privateKey.D.ToString(10)}'
Private key (hex): '{privateKeyHex}'
Public key (x,y): '{restoredPublicKey.Q}'
Public key (hex): '{publicKeyHex}'
");


            string message = "super secret message";
            var signature = CryptoUtilities.SignData(message, restoredPrivateKey);

            bool isValidSignature = CryptoUtilities.VerifySignature(restoredPublicKey, signature, message);
            Console.WriteLine(isValidSignature);
        }

        private static void TestTransaction()
        {
            var senderPrivateKey = CryptoUtilities.PrivateKeyFromPrivateKeyHex("83f919649688da47e81ea3802d49b902d0367b027ca708dbf7a078b844f196b5");
            var publicKey = CryptoUtilities.PublicKeyFromPrivateKey(senderPrivateKey);


            var transaction = new Transaction() { 
                Sender = "04140188cbfa31e9364dcfe0b6204b4ff7913daf66da8cf59eaf4694ed5d3774e424b20a1a95a829976778ac280496dc4cdd1d1fdfab649cba799d084dcf0cc296",
                Recipient = "044842ce6522e4442ccf446c9d28e7be0aa26b83934d60289e3c1f9eba49e44dd6134b7b22ff8a38e86e69683843e3f058f326001ff4fee56e94a1e9681cda4bda",
                Nonce = 1,
                Value = 1000,
                Fee = 5};

            var rawTransaction = transaction.Serialize();
            var forHashing = transaction.SerializeForHashing();

            byte[] forHashingUTF8Bytes = Encoding.UTF8.GetBytes(forHashing);
            byte[] hashBytesManual = CryptoUtilities.CalculateSHA256(forHashingUTF8Bytes);
            string hashManualHEX = Convert.ToHexString(hashBytesManual);

            var hash = transaction.CalculateHash();
            transaction.Hash = hash;

            bool hashMatch = hashManualHEX == hash;

            byte[] hashBytes = Encoding.UTF8.GetBytes(hash);
            byte[] signatureBytes = CryptoUtilities.SignData(hashBytes, senderPrivateKey);

            var transactionSignature = transaction.Sign(senderPrivateKey);
            transaction.Signature = transactionSignature;

            string fullTransaction = transaction.Serialize();

            Console.WriteLine(
$@"
Transaction raw:
'{rawTransaction}'

Transaction for hashing:
'{forHashing}'

Transaction for hashing bytes using UTF8:
'{BytesToString(forHashingUTF8Bytes)}'

Transaction hash bytes:
'{BytesToString(hashBytesManual)}'

Transaction hash:
'{hash}'

Transaction hash bytes using UTF8 for signing:
'{BytesToString(hashBytes)}'

Transaction signature bytes:
'{BytesToString(signatureBytes)}'

Transaction signature:
'{transactionSignature}'

Transaction full:
'{fullTransaction}'

Transaction isValid:
'{transaction.VerifySignature()}'
");


        }
        private static void TestMinerTransaction()
        {
            var senderPrivateKey = CryptoUtilities.PrivateKeyFromPrivateKeyHex("98a980361af2a0574b96342ac0790e243cc032cb4de2de554b8061efc06d35c6");
            var publicKey = CryptoUtilities.PublicKeyFromPrivateKey(senderPrivateKey);


            var transaction = new Transaction()
            {
                Sender = "04fbff67f613b7854c63e5b06eee5e880fd2ff618558e97a1cce73778579a94050c0381b973f34c192cba1662459e00902c2fbd5efd358844f964a94c39f69b91b",
                Recipient = "044842ce6522e4442ccf446c9d28e7be0aa26b83934d60289e3c1f9eba49e44dd6134b7b22ff8a38e86e69683843e3f058f326001ff4fee56e94a1e9681cda4bda",
                Nonce = 1,
                Value = 10,
                Fee = 1
            };

            var rawTransaction = transaction.Serialize();
            var forHashing = transaction.SerializeForHashing();

            byte[] forHashingUTF8Bytes = Encoding.UTF8.GetBytes(forHashing);
            byte[] hashBytesManual = CryptoUtilities.CalculateSHA256(forHashingUTF8Bytes);
            string hashManualHEX = Convert.ToHexString(hashBytesManual);

            var hash = transaction.CalculateHash();
            transaction.Hash = hash;

            bool hashMatch = hashManualHEX == hash;

            byte[] hashBytes = Encoding.UTF8.GetBytes(hash);
            byte[] signatureBytes = CryptoUtilities.SignData(hashBytes, senderPrivateKey);

            var transactionSignature = transaction.Sign(senderPrivateKey);
            transaction.Signature = transactionSignature;

            string fullTransaction = transaction.Serialize();

            Console.WriteLine(
$@"
Transaction raw:
'{rawTransaction}'

Transaction for hashing:
'{forHashing}'

Transaction for hashing bytes using UTF8:
'{BytesToString(forHashingUTF8Bytes)}'

Transaction hash bytes:
'{BytesToString(hashBytesManual)}'

Transaction hash:
'{hash}'

Transaction hash bytes using UTF8 for signing:
'{BytesToString(hashBytes)}'

Transaction signature bytes:
'{BytesToString(signatureBytes)}'

Transaction signature:
'{transactionSignature}'

Transaction full:
'{fullTransaction}'

Transaction isValid:
'{transaction.VerifySignature()}'
");

        }

        private static string BytesToString(byte[] bytes)
        {
            return $"Length['{bytes.Length}'] = '{String.Join(" ", bytes)}'";
        }

        private static void TestBlock()
        {
            Block block = new Block() {
                Index = 1,
                Timestamp = DateTime.UtcNow.ToUnixTime(),
                PreviousBlockHash = "none",
                Difficulty = 5
                };

            block.AddTransactions(new Transaction[]
            {
                new Transaction() {
                Sender = "04140188cbfa31e9364dcfe0b6204b4ff7913daf66da8cf59eaf4694ed5d3774e424b20a1a95a829976778ac280496dc4cdd1d1fdfab649cba799d084dcf0cc296",
                Recipient = "044842ce6522e4442ccf446c9d28e7be0aa26b83934d60289e3c1f9eba49e44dd6134b7b22ff8a38e86e69683843e3f058f326001ff4fee56e94a1e9681cda4bda",
                Nonce = 1,
                Value = 1000,
                Fee = 5 }
            });

            var rawBlock = block.Serialize();
            var blockForHashing = block.SerializeForHashing();
            var hash = block.Mine();

            block.CurrentBlockHash = hash;

            var fullBlock = block.Serialize();


            Console.WriteLine(
$@"
Block raw:
'{rawBlock}'

Block for hashing:
'{blockForHashing}'

Block hash:
'{hash}'

Block full:
'{fullBlock}'
");
        }
    }
}
