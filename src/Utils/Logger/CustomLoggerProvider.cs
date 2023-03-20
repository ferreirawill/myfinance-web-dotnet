using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using myfinance_web_dotnet.utils;

namespace myfinance_web_dotnet.Utils.Logger
{
    public sealed class CustomLoggerProvider : ILoggerProvider
    {
        
        private readonly ConcurrentDictionary<string,CustomLogger> _loggers = new ConcurrentDictionary<string, CustomLogger>();

        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName,name => new CustomLogger(name));
        }

        public void Dispose()
        {

            foreach(var logger in _loggers){

                if (logger.Value is CustomLogger){
                    logger.Value.Dispose();
                }
                
            }
            _loggers.Clear();
        }
    }
}