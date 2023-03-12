
namespace myfinance_web_dotnet.Domain.Entities
{
    public class Transactions
    {
        public int? Id { get; set; }
        public string? Historic { get; set; }
        public decimal Value { get; set; }
        public DateTime Date { get; set; }

        public int PlanoContaId {get;set;} //TODO: RENOMEAR ESSA VARIÁVEL PARA INGLÊS
        public PlanoConta PlanoConta { get; set; } //TODO: RENOMEAR ESSA VARIÁVEL PARA INGLÊS
    }
}