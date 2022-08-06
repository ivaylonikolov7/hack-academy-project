namespace HackChain.Core.Model
{
    public class HackChainSettings
    {
        public string NodeId { get; set; }
        public string BaseUrl { get; set; }
        public IEnumerable<string> KnownPeerNodesBaseUrls { get; set; }
        public int Difficulty { get; set; }
        public long CoinbaseValue { get; set; }
        public string MinersPrivateKeyHex { get; set; }
        public string MinerAddress { get; set; }
    }
}
