using HackChain.Core.Extensions;
using HackChain.Core.Model;
using HackChain.Core.Services;
using HackChain.Utilities;
using Newtonsoft.Json;
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
            TestKeys();

            TestTransaction();
        }

        private static void TestKeys()
        {
            var keypair = CryptoUtilities.GenerateRandomKeys();
            var privateKey = (ECPrivateKeyParameters)keypair.Private;

            var privateKeyHex = CryptoUtilities.PrivateKeyToHexString(privateKey);

            var restoredPrivateKey = CryptoUtilities.PrivateKeyFromPrivateKeyHex(privateKeyHex);

            var restoredPublicKey = CryptoUtilities.PublicKeyFromPrivateKey(restoredPrivateKey);

            var publicKeyBase58 = CryptoUtilities.PublicKeyToBase58(restoredPublicKey);

            //var restoredPublicKeyFromBase58 = CryptoUtilities.PublicKeyFromBase58(publicKeyBase58);

            //Console.WriteLine(restoredPublicKeyFromBase58.Equals(restoredPublicKey));


            Console.WriteLine(
$@"Private key (decimal): '{privateKey.D.ToString(10)}'
Private key (hex): '{privateKeyHex}'
Public key (x,y): '{restoredPublicKey.Q}'
Public key (base58): '{publicKeyBase58}'
");


            string message = "super secret message";
            var signature = CryptoUtilities.SignData(message, restoredPrivateKey);

            bool isValidSignature = CryptoUtilities.VerifySignature(restoredPublicKey, signature, message);
            Console.WriteLine(isValidSignature);
        }

        private static void TestTransaction()
        {
            var senderPrivateKey = CryptoUtilities.PrivateKeyFromPrivateKeyHex("720ca602f9a84617dd99941c8ebdbce52b0f40313982be2eadc4d9a50cc1cc2e");
            var publicKey = CryptoUtilities.PublicKeyFromPrivateKey(senderPrivateKey);


            var transaction = new Transaction(
                sender: "RFsDMTqF4nuaYqbBpPoUGZuGHdbrHTVRxJFJBnj2d36EScekVu39owFUE93zoSm5Y35rTdMDx4ysXTXHaURpBS5u",
                recipient: "RSBbFTCFMDrHgFLkBAthrrp3YwFua2xoALNykhF7LRpRkt2sL8FwFbctwA9X3D5fnunMHZfuLHoc2GVWZZb5mE6B",
                nonce: 1,
                value: 1000,
                fee: 5);

            var rawTransaction = transaction.Serialize();
            var forHashing = transaction.SerializeForHashing();

            var hash = transaction.CalculateHash();
            transaction.Hash = hash;

            var transactionSignature = transaction.Sign(senderPrivateKey);
            transaction.Signature = transactionSignature;

            string fullTransaction = transaction.Serialize();

            Console.WriteLine(
$@"
Transaction raw:
'{rawTransaction}'

Transaction for hashing:
'{forHashing}'

Transaction hash:
'{hash}'

Transaction signature:
'{transactionSignature}'

Transaction full:
'{fullTransaction}'

Transaction isValid:
'{transaction.Verify()}'
");


        }
    }
}
