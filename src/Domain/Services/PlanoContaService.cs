using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using myfinance_web_dotnet.Domain.Entities;
using myfinance_web_dotnet.Domain.Services.Interfaces;
using myfinance_web_dotnet.Models;
using myfinance_web_dotnet.Utils.Logger;

namespace myfinance_web_dotnet.Domain.Services
{
    public class PlanoContaService : IPlanoContaService
    {
        private readonly MyFinanceDbContext _context;

        private readonly ILogger<PlanoContaService> _logger;
        public PlanoContaService(ILogger<PlanoContaService> logger, MyFinanceDbContext context)
        {

            _context = context;
            _logger = logger;
        }

        public List<PlanoContaModel> ListarRegistros()
        {
            var dbSet = _context.PlanoConta;
            var result = new List<PlanoContaModel>();

            foreach (var item in dbSet)
            {

                result.Add(
                    new PlanoContaModel()
                    {
                        Id = item.Id,
                        Descricao = item.Descricao,
                        Tipo = item.Tipo
                    });

            }

            return result;
        }

        public PlanoContaModel RetornaRegistro(int id)
        {
            var item = _context.PlanoConta.Where(x => x.Id == id).First();


            return new PlanoContaModel()
            {
                Id = item.Id,
                Descricao = item.Descricao,
                Tipo = item.Tipo
            };

        }


        public void Salvar(PlanoContaModel planoContaModel)
        {
            var dbSet = _context.PlanoConta;

            var entidate = new PlanoConta()
            {
                Id = planoContaModel.Id,
                Descricao = planoContaModel.Descricao,
                Tipo = planoContaModel.Tipo
            };


            if (entidate.Id == null)
            {

                dbSet.Add(entidate);
                _context.SaveChanges();

                _logger.LogInformation(CustomLoggerEntry.CreateEntry(
                        operacao: EventConstants.Type.Inclusao,
                        tabela: EventConstants.Tablename.PlanoConta,
                        observacao: JsonSerializer.Serialize(new
                        {
                            OldObject = new { },
                            newObject = entidate,
                        }),
                        idRegistro: entidate.Id ?? 0));
            }
            else
            {

                var planoConta = RetornaRegistro(entidate.Id ?? 0);

                dbSet.Attach(entidate);
                
                _context.Entry(entidate).State = EntityState.Modified;
                _context.SaveChanges();

                _logger.LogWarning(CustomLoggerEntry.CreateEntry(
                        operacao: EventConstants.Type.Alteracao,
                        tabela: EventConstants.Tablename.PlanoConta,
                        observacao: JsonSerializer.Serialize(new
                        {
                            OldObject = planoConta,
                            newObject = entidate,
                        }),
                        idRegistro: entidate.Id ?? 0));
            }




        }

        public void Excluir(int id)
        {
            var planoConta = RetornaRegistro(id);

            var dbSet = _context.PlanoConta;
            
            var item = _context.PlanoConta.Where(x => x.Id == id).First();
            _context.Attach(item);
            _context.Remove(item);
            _context.SaveChanges();

            _logger.LogInformation(CustomLoggerEntry.CreateEntry(
                    operacao: EventConstants.Type.Exclusao,
                    tabela: EventConstants.Tablename.PlanoConta,
                    observacao: JsonSerializer.Serialize(new
                    {
                        OldObject = planoConta,
                        newObject = new { },
                    }),
                    idRegistro: id));

        }
    }
}