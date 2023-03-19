using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace myfinance_web_dotnet.Utils.Logger
{
    public class CustomLoggerConfiguration
    {
        public int EventId {get;set;} = 0;

        public LogLevel LogLevel {get;set;} = LogLevel.Information;
    }
}