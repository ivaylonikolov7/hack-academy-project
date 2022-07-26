using HackChain.Utilities;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using System.Text;

namespace HackChain.ConsoleTests
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Test();

            TestHashing();
        }

        private static void Test()
        {
            var keypair = CryptoUtilities.GenerateRandomKeys();
            var privateKey = (ECPrivateKeyParameters)keypair.Private;

            var privateKeyHex = CryptoUtilities.PrivateKeyToHexString(privateKey);

            var restoredPrivateKey = CryptoUtilities.PrivateKeyFromPrivateKeyHex(privateKeyHex);

            var restoredPublicKey = CryptoUtilities.PublicKeyFromPrivateKey(restoredPrivateKey);

            var publicKeyBase58 = CryptoUtilities.PublicKeyToBase58(restoredPublicKey);

            var restoredPublicKeyFromBase58 = CryptoUtilities.PublicKeyFromBase58(publicKeyBase58);

            Console.WriteLine(restoredPublicKeyFromBase58.Equals(restoredPublicKey));


            string message = "super secret message";
            var signature = CryptoUtilities.SignData(message, restoredPrivateKey);

            bool isValidSignature = CryptoUtilities.VerifySignature(restoredPublicKey, signature, message);
            Console.WriteLine(isValidSignature);

        }

        private static void TestHashing()
        {
            string msg = "some message";
            var hash = CryptoUtilities.CalculateSHA256(msg);


            byte[] data = Encoding.UTF8.GetBytes(msg);
            byte[] hashBytes = CryptoUtilities.CalculateSHA256(data);

            var hash1 =  Convert.ToBase64String(hashBytes);

            var hash2 = string.Concat(hashBytes.Select(b => b.ToString("x2")));

            Console.WriteLine(hash1.Equals(hash2));
        }
    }
}
