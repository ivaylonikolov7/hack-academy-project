using HackChain.Core.Interfaces;
using HackChain.Core.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HackChain.Node.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly INodeService _nodeService;
        private readonly IAccountService _accountService;
        private readonly ITransactionService _transactionService;

        public NodeStatus Status { get; set; }
        public IEnumerable<Block> Blocks { get; set; }
        public IEnumerable<Account> Accounts { get; set; }
        public IEnumerable<Transaction> PendingTransactions { get; set; }

        public IndexModel(
            ILogger<IndexModel> logger,
            INodeService nodeService,
            IAccountService accountService,
            ITransactionService transactionService
            )
        {
            _logger = logger;
            _nodeService = nodeService;
            _accountService = accountService;
            _transactionService = transactionService;
        }

        public async Task OnGet()
        {
            Status = await _nodeService.GetNodeStatus();
            Blocks = await _nodeService.GetLast(10);
            Accounts = await _accountService.GetMostFunded(10);
            PendingTransactions = await _transactionService.GetPendingTransactions();
        }
    }
}