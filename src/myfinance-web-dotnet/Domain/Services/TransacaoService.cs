using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using myfinance_web_dotnet.Domain.Entities;
using myfinance_web_dotnet.Domain.Services.Interfaces;
using myfinance_web_dotnet.Models;
using myfinance_web_dotnet.Utils.Logger;

namespace myfinance_web_dotnet.Domain.Services
{
    public class TransacaoService :ITransacaoService
    {
        private readonly MyFinanceDbContext _context;
        private readonly IPlanoContaService _planoContaService;

        private readonly ILogger<TransacaoService> _logger;
        
        public TransacaoService(ILogger<TransacaoService> logger,MyFinanceDbContext context){

            _context = context;
            _logger = logger;
        }

        public List<TransacaoModel> ListarRegistros()
        {
            
            _logger.LogInformation(CustomLoggerEntry.CreateEntry(
                                    operacao: EventConstants.Type.Alteracao,
                                    tabela: EventConstants.Tablename.PlanoConta,
                                    observacao: "texto qualquer", 
                                    idRegistro: 1));

            var dbSet = _context.Transacao.Include(x => x.PlanoConta);
            var result = new List<TransacaoModel>();

            foreach( var item in dbSet){
                
                result.Add(
                    new TransacaoModel(){
                        Id = item.Id,
                        Data = item.Data,
                        Historico = item.Historico,
                        Valor = item.Valor,
                        ItemPlanoConta = new PlanoContaModel(){
                            Id = item.PlanoConta.Id,
                            Descricao = item.PlanoConta.Descricao,
                            Tipo = item.PlanoConta.Tipo
                        },
                        PlanoContaId = item.PlanoContaId
                    });

            }

            return result;
        }

        public TransacaoModel RetornaRegistro(int id)
        {
            var item = _context.Transacao.Where(x=> x.Id == id).First();

            return new TransacaoModel(){
                        Id = item.Id,
                        Data = item.Data,
                        Historico = item.Historico,
                        Valor = item.Valor,
                        PlanoContaId = item.PlanoContaId,
                    };
            
        }

        public void Salvar(TransacaoModel transacaoModel)
        {
            var dbSet = _context.Transacao;

            var entidate = new Transacao(){
                        Id = transacaoModel.Id,
                        Data = transacaoModel.Data,
                        Historico = transacaoModel.Historico,
                        Valor = transacaoModel.Valor,
                        PlanoContaId = transacaoModel.PlanoContaId
                    };


            if (entidate.Id == null){

                dbSet.Add(entidate);

            }else{

                dbSet.Attach(entidate);
                _context.Entry(entidate).State = EntityState.Modified;
            }

            _context.SaveChanges();
        }

        public void Excluir(int id)
        {
           var dbSet = _context.Transacao;

           var item = _context.Transacao.Where(x=> x.Id == id).First();
           _context.Attach(item);
           _context.Remove(item);
           _context.SaveChanges();
           
        }
    }
}