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
        [Route("add")]
        [HttpPost]
        public ActionResult<TransactionDTO> AddTransaction([FromBody] TransactionDTO transaction)
        {


            return transaction;
        }
    }
}
