using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using myfinance_web_dotnet.Domain.Entities;
using myfinance_web_dotnet.Domain.Services.Interfaces;
using myfinance_web_dotnet.Models;
using myfinance_web_dotnet.Utils.Logger;

namespace myfinance_web_dotnet.Domain.Services
{
    public class TransacaoService : ITransacaoService
    {
        private readonly MyFinanceDbContext _context;
        private readonly IPlanoContaService _planoContaService;

        private readonly ILogger<TransacaoService> _logger;

        public TransacaoService(ILogger<TransacaoService> logger, MyFinanceDbContext context)
        {

            _context = context;
            _logger = logger;
        }

        public List<TransacaoModel> ListarRegistros()
        {

            var dbSet = _context.Transacao.Include(x => x.PlanoConta);
            var result = new List<TransacaoModel>();

            foreach (var item in dbSet)
            {

                result.Add(
                    new TransacaoModel()
                    {
                        Id = item.Id,
                        Data = item.Data,
                        Historico = item.Historico,
                        Valor = item.Valor,
                        ItemPlanoConta = new PlanoContaModel()
                        {
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
            var item = _context.Transacao.Where(x => x.Id == id).First();

            return new TransacaoModel()
            {
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

            var entidate = new Transacao()
            {
                Id = transacaoModel.Id,
                Data = transacaoModel.Data,
                Historico = transacaoModel.Historico,
                Valor = transacaoModel.Valor,
                PlanoContaId = transacaoModel.PlanoContaId
            };


            if (entidate.Id == null)
            {

                dbSet.Add(entidate);
                _context.SaveChanges();

                _logger.LogInformation(CustomLoggerEntry.CreateEntry(
                        operacao: EventConstants.Type.Inclusao,
                        tabela: EventConstants.Tablename.Transacao,
                        observacao: JsonSerializer.Serialize(new
                        {
                            OldObject = new { },
                            newObject = entidate,
                        }),
                        idRegistro: entidate.Id ?? 0));

            }
            else
            {
                var transacao = RetornaRegistro(entidate.Id ?? 0);
                
                dbSet.Attach(entidate);
                _context.Entry(entidate).State = EntityState.Modified;
                _context.SaveChanges();
                
                _logger.LogWarning(CustomLoggerEntry.CreateEntry(
                        operacao: EventConstants.Type.Alteracao,
                        tabela: EventConstants.Tablename.Transacao,
                        observacao: JsonSerializer.Serialize(new
                        {
                            OldObject = transacao,
                            newObject = entidate,
                        }),
                        idRegistro: entidate.Id ?? 0));
            }


        }

        public void Excluir(int id)
        {

            var transacao = RetornaRegistro(id);
            var dbSet = _context.Transacao;

            var item = _context.Transacao.Where(x => x.Id == id).First();
            _context.Attach(item);
            _context.Remove(item);
            _context.SaveChanges();



            _logger.LogInformation(CustomLoggerEntry.CreateEntry(
                operacao: EventConstants.Type.Exclusao,
                tabela: EventConstants.Tablename.Transacao,
                observacao: JsonSerializer.Serialize(new
                {
                    OldObject = transacao,
                    newObject = new { },
                }),
                idRegistro: id));

        }
    }
}