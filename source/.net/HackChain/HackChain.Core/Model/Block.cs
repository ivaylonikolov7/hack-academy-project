using System.Numerics;

namespace HackChain.Core.Model
{
    public class Block
    {
        public BigInteger Index { get; set; }
        public BigInteger Timestamp { get; set; }
        public List<Transaction> Data { get; set; }
        public string PreviousBlockHash { get; set; }
        public BigInteger Nonce { get; set; }
        public BigInteger Difficulty { get; set; }
        public string CurrentBlockHash { get; set; }

        public Block(
            BigInteger index,
            BigInteger timestamp,
            string previousBlockHas,
            BigInteger difficulty
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
