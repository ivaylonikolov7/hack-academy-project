using HackChain.Utilities;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;

namespace HackChain.ConsoleTests
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Test();
        }

        private static void Test()
        {
            var keyPair = CryptoUtilities.GenerateRandomKeys();

            BigInteger privateKey = ((ECPrivateKeyParameters)keyPair.Private).D;
            Console.WriteLine("Private key (hex): " + privateKey.ToString(16));
            Console.WriteLine("Private key: " + privateKey.ToString(10));

            ECPublicKeyParameters pubKey = (ECPublicKeyParameters)keyPair.Public;
            Console.WriteLine("Public key: ({0}, {1})",
                pubKey.Q.XCoord.ToBigInteger().ToString(10),
                pubKey.Q.YCoord.ToBigInteger().ToString(10));

            string pubKeyCompressed = CryptoUtilities.EncodeECPointHexCompressed(pubKey.Q);
            Console.WriteLine("Public key (compressed): " + pubKeyCompressed);


            Console.WriteLine($"Public key base58 '{CryptoUtilities.GetPublicKeyFromPrivateKeyExTEST(pubKey)}'");


            //var randomKey = CryptoUtilities.GenerateRandomKeys();
            //Console.WriteLine(randomKey.Private.ToString());

            //var privateKey = (ECPrivateKeyParameters)randomKey.Private;
            //var publicKey = (ECPublicKeyParameters)randomKey.Public;
            //string privateKeyHexString = privateKey.D.ToString(16);

            //Console.WriteLine("Random key pair generated.");
            //Console.WriteLine($"Private key '{privateKey.D.ToString(16)}'");
            //Console.WriteLine($"Public key '{publicKey.Q}'");

            //Console.WriteLine($"Public key base58'{CryptoUtilities.GetPublicKeyFromPrivateKeyExTEST(publicKey)}'");

            //Console.WriteLine(  CryptoUtilities.EncodeECPointHexCompressed(publicKey.Q));

        }
    }
}
