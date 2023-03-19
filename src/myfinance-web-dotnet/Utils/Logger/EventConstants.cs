using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace myfinance_web_dotnet.Utils.Logger
{
    public class EventConstants
    {
        public class Type
        {
            public const string Inclusao = "I";
            public const string Alteracao = "A";
            public const string Exclusao = "E";
        }

        public class Tablename
        {
            public const string PlanoConta = "PlanoConta";
            public const string Transacao = "Transacao";
        }
    }
}