namespace HackChain.Node.Web.DTO
{
    public class BlockDTO
    {
        public long Index { get; set; }
        public long Timestamp { get; set; }
        public ICollection<TransactionDTO> Data { get; set; }
        public string PreviousBlockHash { get; set; }
        public long Nonce { get; set; }
        public long Difficulty { get; set; }
        public string CurrentBlockHash { get; set; }
    }
}
