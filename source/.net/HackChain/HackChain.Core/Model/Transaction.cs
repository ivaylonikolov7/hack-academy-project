using HackChain.Core.Extensions;
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
        public long Nonce { get; set; }
        public string? Data { get; set; }
        public long Value { get; set; }
        public long Fee { get; set; }
        public string Hash { get; set; }
        public string Signature { get; set; }

        public Guid? BlockId { get; set; }
        public virtual Block Block { get; set; }

        public static Transaction Coinbase(string recipient, long nonce, long value)
        {
            var transaction = new Transaction
            {
                Sender = "",
                Recipient = recipient,
                Nonce = nonce,
                Value = value,
                Signature = "Coinbase"
            };

            transaction.Hash = transaction.CalculateHash();

            return transaction;
        }
    }
}
