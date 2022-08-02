namespace HackChain.Node.Web.DTO
{
    public class TransactionDTO
    {
        public string Sender { get; set; }
        public string Recipient { get; set; }
        public decimal Nonce { get; set; }
        public string? Data { get; set; }
        public long Value { get; set; }
        public long Fee { get; set; }
        public string Hash { get; set; }
        public string Signature { get; set; }
    }
}
