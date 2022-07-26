using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Math.EC.Multiplier;
using Org.BouncyCastle.Security;
using SimpleBase;
using System.Text;

namespace HackChain.Utilities
{
    public class CryptoUtilities
    {
        private const string ECAlgorithmName = "ECDSA";
        private static readonly X9ECParameters _curveSecp256k1 = SecNamedCurves.GetByName("secp256k1");
        private static readonly ECDomainParameters _domainParametersSecp256k1 = new ECDomainParameters(_curveSecp256k1.Curve, _curveSecp256k1.G, _curveSecp256k1.N, _curveSecp256k1.H, _curveSecp256k1.GetSeed());

        public static AsymmetricCipherKeyPair GenerateRandomKeys()
        {
            var secureRandom = new SecureRandom();
            var keyParams = new ECKeyGenerationParameters(_domainParametersSecp256k1, secureRandom);

            var generator = new ECKeyPairGenerator(ECAlgorithmName);

            generator.Init(keyParams);

            return generator.GenerateKeyPair();
        }

        public static string PrivateKeyToHexString(ECPrivateKeyParameters privateKey)
        {
            string hex = privateKey.D.ToString(16);

            return hex;
        }

        public static ECPrivateKeyParameters PrivateKeyFromPrivateKeyHex(string privateKeyHex)
        {
            BigInteger pk = new BigInteger(privateKeyHex, 16);
            var privateKey = new ECPrivateKeyParameters(pk, _domainParametersSecp256k1);

            return privateKey;
        }

        public static ECPublicKeyParameters PublicKeyFromPrivateKey(ECPrivateKeyParameters privateKey)
        {
            ECPoint q = new FixedPointCombMultiplier().Multiply(_domainParametersSecp256k1.G, privateKey.D);

            return new ECPublicKeyParameters(q, _domainParametersSecp256k1);
        }

        public static string PublicKeyToBase58(ECPublicKeyParameters publicKey)
        {
            // 65 bytes... is this correct?
            // https://www.tabnine.com/code/java/methods/org.bouncycastle.math.ec.ECPoint/getEncoded - "remove prefix"?
            byte[] publicKeyBytes = publicKey.Q.GetEncoded();

            string result = Base58.Bitcoin.Encode(publicKeyBytes);

            return result;
        }

        public static ECPublicKeyParameters PublicKeyFromBase58(string publicKeyBase58)
        {
            ReadOnlySpan<char> span = new ReadOnlySpan<char>(publicKeyBase58.ToCharArray());
            var publicKeyBytes = Base58.Bitcoin.Decode(span).ToArray();
            ECPoint q = null;// new ECPoint();

            ECPublicKeyParameters publicKey = new ECPublicKeyParameters(q, _domainParametersSecp256k1);

            return publicKey;
        }

        public static string CalculateSHA256(string text)
        {
            byte[] data = Encoding.UTF8.GetBytes(text);
            byte[] hash = CalculateSHA256(data);

            return Convert.ToBase64String(hash);
        }

        public static byte[] CalculateSHA256(byte[] data)
        {
            Sha256Digest digest = new Sha256Digest();
            digest.BlockUpdate(data, 0, data.Length);
            byte[] result = new byte[digest.GetDigestSize()];
            digest.DoFinal(result, 0);

            return result;
        }
        public static string BytesToHex(byte[] bytes)
        {
            return string.Concat(bytes.Select(b => b.ToString("x2")));
        }

        ///// <summary>
        ///// Calculates deterministic ECDSA signature (with HMAC-SHA256), based on secp256k1 and RFC-6979.
        ///// </summary>
        //private static BigInteger[] SignData(byte[] data, ECPrivateKeyParameters privateKey)
        //{
        //    //ECDomainParameters ecSpec = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H);
        //    //ECPrivateKeyParameters keyParameters = new ECPrivateKeyParameters(privateKey, ecSpec);
        //    IDsaKCalculator kCalculator = new HMacDsaKCalculator(new Sha256Digest());
        //    ECDsaSigner signer = new ECDsaSigner(kCalculator);
        //    signer.Init(true, privateKey);
        //    BigInteger[] signature = signer.GenerateSignature(data);
        //    return signature;
        //}

        public static byte[] SignData(byte[] data, ECPrivateKeyParameters privateKey)
        {

            ISigner signer = SignerUtilities.GetSigner("SHA-256withECDSA");
            signer.Init(true, privateKey);
            signer.BlockUpdate(data, 0, data.Length);
            byte[] signatureBytes = signer.GenerateSignature();

            return signatureBytes;
        }

        public static string SignData(string data, ECPrivateKeyParameters privateKey)
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            byte[] signatureBytes = SignData(dataBytes, privateKey);

            return Convert.ToBase64String(signatureBytes);
        }
        public static bool VerifySignature(ECPublicKeyParameters pubKey, string signature, string msg)
        {
            byte[] msgBytes = Encoding.UTF8.GetBytes(msg);
            byte[] sigBytes = Convert.FromBase64String(signature);

            ISigner signer = SignerUtilities.GetSigner("SHA-256withECDSA");
            signer.Init(false, pubKey);
            signer.BlockUpdate(msgBytes, 0, msgBytes.Length);

            return signer.VerifySignature(sigBytes);
        }
    }
}
