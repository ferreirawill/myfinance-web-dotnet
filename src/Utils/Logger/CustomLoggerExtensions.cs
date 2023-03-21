using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;

namespace myfinance_web_dotnet.Utils.Logger
{
    public static class CustomLoggerExtensions
    {
        public static ILoggingBuilder AddCustomLoger(this ILoggingBuilder builder)
        {
            builder.AddConfiguration();
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider,CustomLoggerProvider>());

            LoggerProviderOptions.RegisterProviderOptions<CustomLoggerConfiguration,CustomLoggerProvider>(builder.Services);
            
            
            return builder;

        }
    }
    
}