using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using myfinance_web_dotnet.Domain.Services.Interfaces;
using myfinance_web_dotnet.Models;

namespace myfinance_web_dotnet.Controllers
{
    [Route("[controller]")]
    public class TransacaoController : Controller
    {
        private readonly ILogger<PlanoContaController> _logger;

        private readonly ITransacaoService _transacaoService;
        private readonly IPlanoContaService _planoContaService;

        public TransacaoController(ILogger<PlanoContaController> logger, ITransacaoService transacaoService, IPlanoContaService planoContaService)
        {
            _logger = logger;
            _transacaoService = transacaoService;
            _planoContaService = planoContaService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Transacoes = _transacaoService.ListarRegistros();
            return View();
        }

        [HttpGet]
        [Route("Cadastro")]
        [Route("Cadastro/{id}")]
        public IActionResult Cadastro(int? id)
        {
            var model = new TransacaoModel();

            if(id != null){
                model = _transacaoService.RetornaRegistro((int)id);
            }

            //model.PlanoContas = (IEnumerable<SelectListItem>?) _planoContaService.ListarRegistros();

            var lista = _planoContaService.ListarRegistros();
            model.PlanoContas = new SelectList(lista,"Id","Descricao");

            return View(model);
        }
        
        [HttpPost]
        [Route("Cadastro")]
        [Route("Cadastro/{id}")]
        public IActionResult Cadastro(TransacaoModel transacaoModel)
        {
            _transacaoService.Salvar(transacaoModel);
            return RedirectToAction("Index");
        }
        
        [HttpGet]
        [Route("Excluir/{id}")]
        public IActionResult Excluir(int id)
        {
            _transacaoService.Excluir(id);

            return RedirectToAction("Index");
        }

    }
}