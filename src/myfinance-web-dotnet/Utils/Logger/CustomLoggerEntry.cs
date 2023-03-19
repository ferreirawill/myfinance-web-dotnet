

using System.ComponentModel.DataAnnotations.Schema;

namespace myfinance_web_dotnet.Utils.Logger
{
    public class CustomLoggerEntry
    {        
        public int? Id {get;set;}
        public DateTime Data {get;set;}
        public string Operacao {get;set;}
        public string Observacao {get;set;}
        public string Tabela {get;set;}

        [Column("id_registro")]
        public int IdRegistro {get;set;}

    }

}

