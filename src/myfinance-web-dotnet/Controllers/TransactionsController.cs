using Microsoft.AspNetCore.Mvc;
using myfinance_web_dotnet.Models;

namespace myfinance_web_dotnet.Controllers
{
    [Route("[controller]")]
    public class TransactionsController : Controller
    {
        private readonly ILogger<PlanoContaController> _logger;

        public TransactionsController(ILogger<PlanoContaController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("Cadastro")]
        public IActionResult Register()
        {
            return View();
        }
        
        [HttpPost]
        [Route("Cadastro")]
        public IActionResult Register(TransactionModel planoContaModel)
        {
            return RedirectToAction("index");
        }
 

    }
}