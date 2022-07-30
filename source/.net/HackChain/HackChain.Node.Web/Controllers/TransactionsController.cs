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

        public TransactionsController(
            INodeService nodeService)
        {
            _nodeService = nodeService;
        }
        [Route("add")]
        [HttpPost]
        public async Task<ActionResult<Transaction>> AddTransaction([FromBody] Transaction transaction)
        {

            await _nodeService.AddTransaction(transaction);

            return transaction;
        }
    }
}
