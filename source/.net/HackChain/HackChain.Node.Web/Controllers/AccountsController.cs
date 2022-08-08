using AutoMapper;
using HackChain.Core.Interfaces;
using HackChain.Node.DTO;
using Microsoft.AspNetCore.Mvc;

namespace HackChain.Node.Web.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private IAccountService _accountService;
        private ITransactionService _transactionService;
        private IMapper _mapper;

        public AccountsController(
            IAccountService accountService,
            ITransactionService transactionService,
            IMapper mapper)
        {
            _accountService = accountService;
            _transactionService = transactionService;
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

        [Route("{address}/transactions")]
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<TransactionWithBlockInfoDTO>>>> GetTransactionsByAddress(string address)
        {
            var transactions = await _transactionService.GetTransactionByAddress(address);
            var result = _mapper.Map<IEnumerable<TransactionWithBlockInfoDTO>>(transactions);


            return ApiResponse<IEnumerable<TransactionWithBlockInfoDTO>>.Successful(result);
        }
    }
}
