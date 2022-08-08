using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackChain.Node.DTO
{
    public class TransactionWithBlockInfoDTO : TransactionDTO
    {
        public long BlockIndex { get; set; }
    }
}
