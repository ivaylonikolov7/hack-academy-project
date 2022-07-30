using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackChain.Core.Model
{
    public class Account
    {
        public string Address { get; set; }
        public decimal Balance { get; set; }
        public long Nonce { get; set; }
    }
}
