using HackChain.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackChain.Core.Interfaces
{
    public interface INodeConnector
    {
        Task<Block> GetBlockByIndex(long index);
        Task<NodeStatus> GetNodeStatus();
        Task<IEnumerable<PeerNode>> GetPeerNodes();
        Task AddPeerNode(PeerNode peerNode);
        Task AddBlock(Block block);
        Task AddTransaction(Transaction transaction);
        Task GetPendingTransactions()
    }
}
