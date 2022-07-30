using AutoMapper;
using HackChain.Core.Interfaces;
using HackChain.Core.Model;
using HackChain.Node.Web.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HackChain.Node.Web.Controllers
{
    [Route("api/transactions")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private INodeService _nodeService;
        private IMapper _mapper;

        public TransactionsController(
            INodeService nodeService,
            IMapper mapper)
        {
            _nodeService = nodeService;
            _mapper = mapper;
        }
        [Route("add")]
        [HttpPost]
        public async Task<ActionResult<TransactionDTO>> AddTransaction([FromBody] TransactionDTO transaction)
        {
            var internalTransaction = _mapper.Map<Transaction>(transaction);
            await _nodeService.AddTransaction(internalTransaction);

            return transaction;
        }
    }
}
