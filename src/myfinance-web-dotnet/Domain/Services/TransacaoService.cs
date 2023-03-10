using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using myfinance_web_dotnet.Domain.Entities;
using myfinance_web_dotnet.Domain.Services.Interfaces;
using myfinance_web_dotnet.Models;

namespace myfinance_web_dotnet.Domain.Services
{
    public class TransacaoService :ITransacaoService
    {
        private readonly MyFinanceDbContext _context;
        private readonly IPlanoContaService _planoContaService;
        
        public TransacaoService(MyFinanceDbContext context){

            _context = context;
        }

        public List<TransacaoModel> ListarRegistros()
        {
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