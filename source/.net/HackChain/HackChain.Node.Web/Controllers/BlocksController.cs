﻿using AutoMapper;
using HackChain.Core.Interfaces;
using HackChain.Node.DTO;
using Microsoft.AspNetCore.Mvc;

namespace HackChain.Node.Web.Controllers
{
    [Route("api/blocks")]
    [ApiController]
    public class BlocksController : ControllerBase
    {
        private INodeService _nodeService;
        private IMapper _mapper;

        public BlocksController(
            INodeService nodeService,
            IMapper mapper)
        {
            _nodeService = nodeService;
            _mapper = mapper;
        }

        [Route("{index}")]
        [HttpGet]
        public async Task<ActionResult<ApiResponse<BlockDTO>>> GetByIndex(long index)
        {
            var block = await _nodeService.GetBlockByIndex(index);
            var result = _mapper.Map<BlockDTO>(block);

            return ApiResponse<BlockDTO>.Successful(result);
        }

        [Route("mine")]
        [HttpPost]
        public async Task<ActionResult<ApiResponse<BlockDTO>>> Mine()
        {
            var block = await _nodeService.MineBlock();
            var result = _mapper.Map<BlockDTO>(block);

            return ApiResponse<BlockDTO>.Successful(result);
        }

        [Route("add")]
        [HttpPost]
        public async Task<ActionResult<ApiResponse<bool>>> AddBlock(AddBlockDTO blockCandidate)
        {
            await _nodeService.TryAddBlock(blockCandidate.BlockIndex, "");

            return ApiResponse<bool>.Successful(true);
        }
    }
}
