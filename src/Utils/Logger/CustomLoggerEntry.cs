

using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

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



        public static string CreateEntry(string operacao, string tabela,string observacao,int idRegistro){
            return JsonSerializer.Serialize(new CustomLoggerEntry(){
                IdRegistro=idRegistro,
                Data=DateTime.Now,
                Observacao=observacao,
                Operacao=operacao,
                Tabela=tabela
            });
        }

        public static CustomLoggerEntry DeserializeEntry(string customEntry){
            return JsonSerializer.Deserialize<CustomLoggerEntry>(customEntry);
        }

    }

}

