using HackChain.Core.Interfaces;
using HackChain.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackChain.Core.Services
{
    public class NodeService: INodeService
    {
        public Task AddTransaction(Transaction transaction)
        {
            return Task.CompletedTask;
        }
    }
}
