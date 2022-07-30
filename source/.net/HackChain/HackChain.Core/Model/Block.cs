using System.Numerics;

namespace HackChain.Core.Model
{
    public class Block
    {
        public long Index { get; set; }
        public long Timestamp { get; set; }
        public ICollection<Transaction> Data { get; set; }
        public string PreviousBlockHash { get; set; }
        public long Nonce { get; set; }
        public long Difficulty { get; set; }
        public string CurrentBlockHash { get; set; }
    }
}
