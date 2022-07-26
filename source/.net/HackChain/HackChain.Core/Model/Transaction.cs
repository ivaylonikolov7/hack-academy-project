using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace HackChain.Core.Model
{
    public class Transaction
    {
        public string Sender { get; set; }
        public string Recipient { get; set; }
        public BigInteger Nonce { get; set; }
        public string Data { get; set; }
        public BigInteger Value { get; set; }
        public BigInteger Fee { get; set; }
        public string Hash { get; set; }
        public string Signature { get; set; }
        public Transaction(
            string sender,
            string recipient,
            BigInteger nonce,
            BigInteger value,
            BigInteger fee
            )
        {
            Sender = sender;
            Recipient = recipient;
            Nonce = nonce;
            Value = value;
            Fee = fee;
        }
    }
}
