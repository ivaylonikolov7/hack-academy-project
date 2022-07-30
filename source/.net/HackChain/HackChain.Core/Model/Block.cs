using System.Numerics;

namespace HackChain.Core.Model
{
    public class Block
    {
        public decimal Index { get; set; }
        public decimal Timestamp { get; set; }
        public List<Transaction> Data { get; set; }
        public string PreviousBlockHash { get; set; }
        public decimal Nonce { get; set; }
        public decimal Difficulty { get; set; }
        public string CurrentBlockHash { get; set; }

        public Block(
            decimal index,
            decimal timestamp,
            string previousBlockHas,
            decimal difficulty
            )
        {
            Index = index;
            Timestamp = timestamp;
            Data = new List<Transaction>();
            PreviousBlockHash = previousBlockHas;
            Difficulty = difficulty;
        }
    }
}
