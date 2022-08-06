using AutoMapper;
using HackChain.Core.Interfaces;
using HackChain.Core.Model;
using HackChain.Node.DTO;
using Microsoft.AspNetCore.Mvc;

namespace HackChain.Node.Web.Controllers
{
    [Route("api/faucet")]
    [ApiController]
    public class FaucetController : ControllerBase
    {
        private IWalletService _walletService;
        private ITransactionService _transactionService;
        private HackChainSettings _settings;
        private IMapper _mapper;

        public FaucetController(
            IWalletService walletService,
            ITransactionService transactionService,
            HackChainSettings settings,
            IMapper mapper)
        {
            _walletService = walletService;
            _transactionService = transactionService;
            _settings = settings;
            _mapper = mapper;
        }

        [Route("fund/{address}")]
        [HttpPost]
        public async Task<ActionResult<ApiResponse<TransactionDTO>>> FundAddress(string address)
        {
            _walletService.SetPrivateKey(_settings.MinersPrivateKeyHex);
            var transaction = await _walletService.GenerateTransaction(address, 20, 1);
            await _transactionService.AddTransaction(transaction);
            var result = _mapper.Map<TransactionDTO>(transaction);


            return ApiResponse<TransactionDTO>.Successful(result);
        }
    }
}
