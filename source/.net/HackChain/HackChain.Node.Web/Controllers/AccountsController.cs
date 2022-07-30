using AutoMapper;
using HackChain.Core.Interfaces;
using HackChain.Node.Web.DTO;
using Microsoft.AspNetCore.Mvc;

namespace HackChain.Node.Web.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private INodeService _nodeService;
        private IMapper _mapper;

        public AccountsController(
            INodeService nodeService,
            IMapper mapper)
        {
            _nodeService = nodeService;
            _mapper = mapper;
        }

        [Route("{address}")]
        [HttpGet]
        public async Task<ActionResult<AccountDTO>> GetByAddress(string address)
        {
            var account = await _nodeService.GetAccountByAddress(address);
            var result = _mapper.Map<AccountDTO>(account);


            return result;
        }
    }
}
