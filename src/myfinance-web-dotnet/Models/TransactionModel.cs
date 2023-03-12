using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using myfinance_web_dotnet.Domain.Entities;

namespace myfinance_web_dotnet.Models
{
    public class TransactionModel
    {
        public int? Id { get; set; }
        public string? Historic { get; set; }
        public decimal Value { get; set; }
        public DateTime Date { get; set; }

        public int PlanoContaId {get;set;} //TODO: RENOMEAR ESSA VARIÁVEL PARA INGLÊS
        public IEnumerable<SelectListItem>? PlanoConta { get; set; } //TODO: RENOMEAR ESSA VARIÁVEL PARA INGLÊS
    }
}