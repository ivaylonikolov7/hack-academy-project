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
        public decimal Value { get; set; }
        public decimal Fee { get; set; }
        public string Hash { get; set; }
        public string Signature { get; set; }

        public long? BlockIndex { get; set; }
        public virtual Block Block { get; set; }
    }
}
