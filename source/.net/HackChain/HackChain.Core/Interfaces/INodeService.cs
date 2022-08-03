using HackChain.Core.Model;

namespace HackChain.Core.Interfaces
{
    public interface INodeService
    {

        Task<Block> GetBlockByIndex(long index);
        Task<Block> MineBlock();
        Task<NodeStatus> GetNodeStatus();
    }
}
