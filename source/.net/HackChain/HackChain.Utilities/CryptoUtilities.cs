using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Security;
using SimpleBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackChain.Utilities
{
    public class CryptoUtilities
    {
        static readonly X9ECParameters curve = SecNamedCurves.GetByName("secp256k1");

        public static AsymmetricCipherKeyPair GenerateRandomKeys(int keySize = 256)
        {
            ECKeyPairGenerator gen = new ECKeyPairGenerator();
            SecureRandom secureRandom = new SecureRandom();
            KeyGenerationParameters keyGenParam = new KeyGenerationParameters(secureRandom, keySize);
            gen.Init(keyGenParam);

            return gen.GenerateKeyPair();
        }

        public static ECPoint GetPublicKeyFromPrivateKey(BigInteger privKey)
        {
            ECPoint pubKey = curve.G.Multiply(privKey).Normalize();

            return pubKey;
        }

        public static string EncodeECPointHexCompressed(ECPoint point)
        {
            BigInteger x = point.XCoord.ToBigInteger();
            BigInteger y = point.YCoord.ToBigInteger();
            return x.ToString(16) + Convert.ToInt32(y.TestBit(0));
        }

        public static string GetPublicKeyFromPrivateKeyExTEST(ECPublicKeyParameters publicKey)

        {
            string result = Base58.Bitcoin.Encode(publicKey.Q.GetEncoded());
            return result;
        }

        ///// <summary>
        ///// Generates random private key
        ///// </summary>
        ///// <param name="keySize"></param>
        ///// <returns></returns>
        //public static AsymmetricCipherKeyPair GenerateRandomKeys(int keySize = 256)
        //{
        //    ECKeyPairGenerator gen = new ECKeyPairGenerator("secp256k1");
        //    SecureRandom secureRandom = new SecureRandom();
        //    KeyGenerationParameters keyGenParam = new KeyGenerationParameters(secureRandom, keySize);
        //    gen.Init(keyGenParam);

        //    return gen.GenerateKeyPair();
        //}



        //store it in decimal or hex format
        //load private key from decimal or hex
        //generate public key from private key
        // - pair or base58 encoded
        //restore public key from pair or base58 encoded
        //sign text message using private kye
        //verify signature using public key






















        //public static string GetPublicKeyFromPrivateKeyEx(string privateKey)

        //{
        //    var curve = SecNamedCurves.GetByName("secp256k1");
        //    var domain = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H);

        //    var d = new BigInteger(privateKey);
        //    var q = domain.G.Multiply(d);

        //    var publicKey = new ECPublicKeyParameters(q, domain);

        //    string result = Base58.Bitcoin.Encode(publicKey.Q.GetEncoded());
        //    return result;
        //}



        //public static bool VerifySignature(string message, string publicKey, string signature)
        //{
        //    var curve = SecNamedCurves.GetByName("secp256k1");
        //    var domain = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H);

        //    var publicKeyBytes = Base58.Bitcoin.Decode(publicKey);
        //    var q = curve.Curve.DecodePoint(publicKeyBytes.ToArray());
        //    var keyParameters = new ECPublicKeyParameters(q, domain);

        //    ISigner signer = SignerUtilities.GetSigner("SHA-256withECDSA");
        //    signer.Init(false, keyParameters);
        //    signer.BlockUpdate(Encoding.Unicode.GetBytes(message), 0, message.Length);


        //    var signatureBytes = Base58.Bitcoin.Decode(signature);

        //    return signer.VerifySignature(signatureBytes.ToArray());
        //}
    }
}
