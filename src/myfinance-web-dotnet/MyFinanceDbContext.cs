using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using myfinance_web_dotnet.Domain.Entities;

namespace myfinance_web_dotnet
{
    public class MyFinanceDbContext : DbContext
    {
        public DbSet<PlanoConta> PlanoConta { get; set; }
        public DbSet<Transacao> Transacao { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost,1433;Database=myfinance;User Id=sa;Password=Pos@123456;Encrypt=True;TrustServerCertificate=True;");
        }
    }
}