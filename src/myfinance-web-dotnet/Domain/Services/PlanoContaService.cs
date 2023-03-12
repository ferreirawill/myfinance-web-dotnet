using Microsoft.EntityFrameworkCore;
using myfinance_web_dotnet.Domain.Entities;
using myfinance_web_dotnet.Domain.Services.Interfaces;
using myfinance_web_dotnet.Models;

namespace myfinance_web_dotnet.Domain.Services
{
    public class PlanoContaService :IPlanoContaService
    {
        private readonly MyFinanceDbContext _context;
        
        public PlanoContaService(MyFinanceDbContext context){

            _context = context;
        }

        public List<PlanoContaModel> ListarRegistros()
        {
            var dbSet = _context.PlanoConta;
            var result = new List<PlanoContaModel>();

            foreach( var item in dbSet){
                
                result.Add(
                    new PlanoContaModel(){
                        Id = item.Id,
                        Descricao = item.Descricao,
                        Tipo = item.Tipo
                    });

            }

            return result;
        }

        public PlanoContaModel RetornaRegistro(int id)
        {
            var item = _context.PlanoConta.Where(x=> x.Id == id).First();

            return new PlanoContaModel(){
                Id= item.Id,
                Descricao = item.Descricao,
                Tipo= item.Tipo
            };
            
        }

        public void Salvar(PlanoContaModel planoContaModel)
        {
            var dbSet = _context.PlanoConta;

            var entidate = new PlanoConta(){
                Id = planoContaModel.Id,
                Descricao = planoContaModel.Descricao,
                Tipo = planoContaModel.Tipo
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
           var dbSet = _context.PlanoConta;

           var item = _context.PlanoConta.Where(x=> x.Id == id).First();
           _context.Attach(item);
           _context.Remove(item);
           _context.SaveChanges();
           
        }
    }
}