# My Finance Web
Projeto para controle de finanças pessoas


O **My Finance** é um projeto desenvolvido em Aspnet MVC que tem como objetivo oferecer um controle financeiro pessoal, permitindo o registro de transações e categorizando-as dentro de um plano de conta. Como features adicionais, foram inseridos um alerta para confirmação de exclusão de itens pelo usuário e o registro de logs no banco de dados para todas as operações de escrita.


### Tecnlogias utilizadas
<p align="left" style="'margin':'50px'">
    <a href="https://dotnet.microsoft.com/pt-br/" targer="_blank"><img src="https://cdn.iconscout.com/icon/free/png-256/microsoft-dot-net-1-1175179.png" width="50" height="50"></a>
    &nbsp
    &nbsp
    <a href="https://hub.docker.com/_/microsoft-mssql-server" targer="_blank"><img src="https://cdn.iconscout.com/icon/free/png-256/docker-226091.png?f=webp&amp;w=256" width="50" height="50"></a>
    &nbsp
    &nbsp
    <a href="https://www.microsoft.com/pt-br/sql-server/sql-server-2022" targer="_blank"><img src="https://cdn-icons-png.flaticon.com/512/5968/5968364.png" width="50" height="50"></a>
</p>


## Arquitetura da solução

A arquitetura do projeto é baseada em MVC com a separação em duas camadas com suas responsabilidades distintas:

- Aplicação
    - Camada contendo os componentes do MVC padrão, responsável por processar as entradas de usuários, efetuar a lógica da aplicação e tomar decisões sobre as saídas de informações.
- Domínio
    - Camada contendo os servios e entidades da aplicação, responsável por representar os conceitos do mundo real e as regras de domínio do problema. 




![Arquitetura da solução utilizando MVC como uma camada de aplicação e a camada de domínio para regras de negócio](docs/my-finance-web-arch.png)
## Modelo de entidade e relacionamentos (MER)
![Modelo de entidade e relacionamentos (MER)](docs/erd.png)


## Execução
O projeto foi desenvolvido para ser executado localmente, e por isso não há uma estrutura de deploy definida. Para executar o projeto cerifique-se de possuir o [.NET 6.0](https://dotnet.microsoft.com/pt-br/download/dotnet/6.0) e o [Docker](https://docs.docker.com/engine/install/) instalados.
Após clonar o repositório execute comando:

``` zsh
~$ docker-compose up
```
Com o container do SQL Server rodando, conecte-se ao banco e  execute as queries disponíveis nos arquivos [docs/myfinance-queries.sql](/docs/myfinance-queries.sql) e [docs/feature-extra-query.sql](/docs/feature-extra-query.sql). Após executar as queries, execute o projeto com o seguinte comando



``` zsh
~$ dotnet run watch 
```

## Detalhes da implementação da API de Logging do .NET

A implementação de um provedor de logs personalizado do .NET seguiu a [documentação oficial](https://learn.microsoft.com/pt-br/dotnet/core/extensions/custom-logging-provider) disponível no site da Microsoft. Algumas definições da documentação foram adaptadas para a utilização nesse projeto.


Inicialmente, a classe de configuração do log customizado foi definida com valores padrão para o EventId e o LogLevel. Isso porque essa aplicação será executada apenas em modo de debug e assim não há necessidade de buscar as configurações de log do ```app.settings.json```

``` c#
    public class CustomLoggerConfiguration
    {
        public int EventId {get;set;} = 0;

        public LogLevel LogLevel {get;set;} = LogLevel.Information;
    }
```



A utilização da interface ILogger necessita da implementação de 3 métodos. Os métodos e a forma que foram implementados são explicados no tópico abaixo:

- ```BeginScope<TState>(TState)```
    - Não era o objetivo trabalhar com escopos de log nesse projeto, por isso sempre é retornado ```null```
- ```IsEnabled(LogLevel)```
    - Neste caso o log sempre estará habilitado e por isso é sempre retornado ```true```
- ```Log<TState>(LogLevel, EventId, TState, Exception, Func<TState,Exception,String>)```
    - Executa a gravação de entradas de log no banco de dados e também imprime no console.

Além desses métodos, foi criado um método ```Dispose()``` com o objetivo de fazer a liberação de recursos da instância do banco de dados que é criada para a escrita dos logs. A classe que implementa essa interface possui a seguinte característica:

``` c#
    public sealed class CustomLogger : ILogger
    {
        private MyFinanceDbContext _context = new MyFinanceDbContext();
        public string name;


        public CustomLogger(string name)
        {
            this.name = name; 
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;  // Para essa feature os escopos de logs não serão abordados

        public bool IsEnabled(LogLevel logLevel) => true; // Para essa feature o log estará sempre habilitado

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            //Implementa lógica para escrever logs no console e também no banco de dados
        }

        public void Dispose(){
            _context.Dispose(); // Libera recursos da instância do banco de dados
        }
    }
```


Para a criação de uma nova instância de `CustomLogger` é necessário criar uma clase que implemente o `ILoggerProvider`. Essa implementação requer a implementação dos seguintes métodos.

* `CreateLogger(string categoryName)` 
    - Responsável por criar e retornar um objeto ILogger. Neste projeto permitimos a criação de N logs com `categoryName` diferentes.
* `Dispose()`
    -  Método responsável por fazer a liberação de recursos do ILogger. Nessa implementação ao ser acionado será executada uma iteração no dicionário de loggers para que sejam liberados os recursos de cada uma das instâncias criadas
    -  Vem da implementação do `IDisposable` por `ILoggerProvider`


A classe que implementa o ILoggerProvider possui a característica do trecho de código abaixo:
``` c#
    public sealed class CustomLoggerProvider : ILoggerProvider
    {
        
        private readonly ConcurrentDictionary<string,CustomLogger> _loggers = new ConcurrentDictionary<string, CustomLogger>();

        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName,name => new CustomLogger(name)); //Cria ou recupera a instância de log com a categoria definida;
        }

        public void Dispose()
        {

            foreach(var logger in _loggers){

                if (logger.Value is CustomLogger){
                    logger.Value.Dispose(); //Libera recursos dos logs criados
                }
                
            }
            _loggers.Clear(); // Limpa dicionário e logs
        }
    }
```


Para que fosse possível registrar um agente personalizado para a criação dos logs foi criada a classe estática `CustomLoggerExtensions` com o método `AddCustomLogger(this ILoggingBuilder builder)` que efetua a configuração do serviço de logs.
```c#
    public static class CustomLoggerExtensions
    {
        public static ILoggingBuilder AddCustomLogger(this ILoggingBuilder builder)
        {
            builder.AddConfiguration();
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider,CustomLoggerProvider>());

            LoggerProviderOptions.RegisterProviderOptions<CustomLoggerConfiguration,CustomLoggerProvider>(builder.Services);
            
            
            return builder;

        }
    }
```


Para registrar o agente personalizado foi utilizado o seguinte comando em `Program.cs`:
```c# 
var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddCustomLogger();
```