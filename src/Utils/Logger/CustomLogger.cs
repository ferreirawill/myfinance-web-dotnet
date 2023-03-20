using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using myfinance_web_dotnet.Utils.Logger;

namespace myfinance_web_dotnet.utils
{
    public sealed class CustomLogger : ILogger
    {
        private MyFinanceDbContext _context = new MyFinanceDbContext();
        public string name;


        public CustomLogger(string name)
        {
            this.name = name;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {

            var logObject = new
            {
                LogLevel = logLevel.ToString(),
                EventId = eventId.Id,
                Exception = exception,
                logObject = formatter(state, exception).Replace("\"", "")
            };

            string jsonLogObject = JsonSerializer.Serialize(logObject);
           
            try
            {
                  Console.WriteLine(jsonLogObject);
                  _context.Log.Add(CustomLoggerEntry.DeserializeEntry(formatter(state, exception)));
                  _context.SaveChanges();
            }
            catch (System.Exception e)
            {
                // Don't care
            }
        }

        public void Dispose(){
            _context.Dispose();
        }
    }
}