using AutoMapper;
using HackChain.Core.Interfaces;
using HackChain.Node.Web.DTO;
using HackChain.Node.Web.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace HackChain.Node.Web.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private IAccountService _accountService;
        private IMapper _mapper;

        public AccountsController(
            IAccountService accountService,
            IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        [Route("{address}")]
        [HttpGet]
        public async Task<ActionResult<ApiResponse<AccountDTO>>> GetByAddress(string address)
        {
            var account = await _accountService.GetAccountByAddress(address);
            var result = _mapper.Map<AccountDTO>(account);


            return ApiResponse<AccountDTO>.Successful(result);
        }
    }
}
