﻿using HackChain.Core.Model;

namespace HackChain.Core.Interfaces
{
    public interface INodeService
    {
        Task Init();
        Task<Block> GetBlockByIndex(long index);
        Task<IEnumerable<Block>> GetLast(int count);    
        Task<Block> MineBlock();
        Task<NodeStatus> GetNodeStatus();
        Task<IEnumerable<PeerNode>> GetPeerNodes();
        Task<bool> TryAddPeerNode(string peerNodeUrl);
        Task TryAddBlock(long remoteBlockIndex, string peerNodeUrl);
        void PropagateTransaction(Transaction transaction);
        void PropagateBlock(Block block);
    }
}
