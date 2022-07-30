using HackChain.Core.Data;
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
        private HackChainDbContext _db;

        public NodeService(HackChainDbContext db)
        {
            _db = db;
        }
        public async Task AddTransaction(Transaction transaction)
        {
            _db.Transactions.Add(transaction);
            
            await _db.SaveChangesAsync();
        }
    }
}
