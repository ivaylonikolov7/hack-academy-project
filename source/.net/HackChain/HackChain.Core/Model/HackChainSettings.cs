namespace HackChain.Core.Model
{
    public class HackChainSettings
    {
        public int Difficulty { get; set; }
        public long CoinbaseValue { get; set; }
        public string MinersPrivateKeyHex { get; set; }
        public string MinerAddress { get; set; }
    }
}
