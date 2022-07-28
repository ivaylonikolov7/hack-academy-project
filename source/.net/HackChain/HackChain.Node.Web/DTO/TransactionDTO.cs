namespace HackChain.Node.Web.DTO
{
    public class TransactionDTO
    {
        public string Sender { get; set; }
        public string Recipient { get; set; }
        public string Nonce { get; set; }
        public string? Data { get; set; }
        public string Value { get; set; }
        public string Fee { get; set; }
        public string Hash { get; set; }
        public string Signature { get; set; }
    }
}
