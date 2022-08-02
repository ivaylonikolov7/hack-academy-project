using AutoMapper;
using HackChain.Core.Interfaces;
using HackChain.Core.Model;
using HackChain.Node.Web.DTO;
using Microsoft.AspNetCore.Mvc;

namespace HackChain.Node.Web.Controllers
{
    [Route("api/transactions")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private ITransactionService _transactionService;
        private IMapper _mapper;

        public TransactionsController(
            ITransactionService transactionService,
            IMapper mapper)
        {
            _transactionService = transactionService;
            _mapper = mapper;
        }

        [Route("add")]
        [HttpPost]
        public async Task<ActionResult<TransactionDTO>> Add([FromBody] TransactionDTO transaction)
        {
            var internalTransaction = _mapper.Map<Transaction>(transaction);
            await _transactionService.AddTransaction(internalTransaction);

            return transaction;
        }

        [Route("{hash}")]
        [HttpGet]
        public async Task<ActionResult<TransactionDTO>> GetByHash(string hash)
        {
            var transaction = await _transactionService.GetTransactionByHash(hash);
            var result = _mapper.Map<TransactionDTO>(transaction);


            return result;
        }

        [Route("pending")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionDTO>>> GetPending()
        {
            var transactions = await _transactionService.GetPendingTransactions();
            List<TransactionDTO> result = _mapper.Map<List<TransactionDTO>>(transactions);

            return result;
        }
    }
}
