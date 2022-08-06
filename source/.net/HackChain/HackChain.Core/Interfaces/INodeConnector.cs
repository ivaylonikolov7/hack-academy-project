using HackChain.Core.Model;
using HackChain.Node.DTO;

namespace HackChain.Core.Interfaces
{
    public interface INodeConnector
    {
        Task<BlockDTO> GetBlockByIndex(long index);
        Task<NodeStatusDTO> GetNodeStatus();
        Task<IEnumerable<PeerNodeDTO>> GetPeerNodes();
        Task<bool> AddPeerNode(PeerNodeDTO peerNode);
        Task<bool> AddBlock(BlockDTO block);
        Task<bool> AddTransaction(TransactionDTO transaction);
        Task<IEnumerable<TransactionDTO>> GetPendingTransactions();
    }
}
