using HackChain.Core.Model;

namespace HackChain.Core.Interfaces
{
    public interface INodeService
    {
        Task Init();
        Task<Block> GetBlockByIndex(long index);
        Task<Block> MineBlock();
        Task<NodeStatus> GetNodeStatus();
        Task<IEnumerable<PeerNode>> GetPeerNodes();
        Task<bool> TryAddPeerNode(string peerNodeUrl);
        Task TryAddBlock(Block remoteBlock, string peerNodeUrl);
        void PropagateTransaction(Transaction transaction);
        void PropagateBlock(Block block);
    }
}
