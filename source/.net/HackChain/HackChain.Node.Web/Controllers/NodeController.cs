using AutoMapper;
using HackChain.Core.Interfaces;
using HackChain.Core.Model;
using HackChain.Node.Web.DTO;
using HackChain.Node.Web.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace HackChain.Node.Web.Controllers
{
    [Route("api/node")]
    [ApiController]
    public class NodeController : ControllerBase
    {
        private INodeService _nodeService;
        private IMapper _mapper;

        public NodeController(
            INodeService nodeService,
            IMapper mapper)
        {
            _nodeService = nodeService;
            _mapper = mapper;
        }

        [Route("status")]
        [HttpGet]
        public async Task<ActionResult<ApiResponse<NodeStatus>>> GetStatus()
        {
            var nodeStatus = await _nodeService.GetNodeStatus();

            return ApiResponse<NodeStatus>.Successful(nodeStatus);
        }

        [Route("peers")]
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<PeerNodeDTO>>>> GetPeerNodes()
        {
            var peerNodes = await _nodeService.GetPeerNodes();
            var result = _mapper.Map<IEnumerable<PeerNodeDTO>>(peerNodes);

            return ApiResponse<IEnumerable<PeerNodeDTO>>.Successful(result);
        }

        [Route("peers")]
        [HttpPost]
        public async Task<ActionResult<ApiResponse<bool>>> PostPeerNode(PeerNodeDTO peerNode)
        {
            var domainPeerNode = _mapper.Map<PeerNode>(peerNode);
            _nodeService.AddPeerNode(domainPeerNode);

            return ApiResponse<bool>.Successful(true);
        }
    }
}
