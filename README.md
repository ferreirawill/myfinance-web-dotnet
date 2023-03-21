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
![Arquitetura da solução utilizando MVC como uma camada de aplicação e a camada de domínio para regras de negócio](docs/my-finance-web-arch.png)
## Modelo de entidade e relacionamentos (MER)
![Modelo de entidade e relacionamentos (MER)](docs/erd.png)


## Execução
O projeto foi desenvolvido para ser executado localmente, e por isso, não há uma estrutura de deploy definida. Para executar o projeto cerifique-se de possuir o [.NET 6.0](https://dotnet.microsoft.com/pt-br/download/dotnet/6.0) e o [Docker](https://docs.docker.com/engine/install/) instalados.
Após clonar o repositório execute comando:

``` zsh
~$ docker-compose up
```
Com o container do SQL Server rodando, conecte-se ao banco e  execute as queries disponíveis nos arquivos [docs/myfinance-queries.sql](/docs/myfinance-queries.sql) e [docs/feature-extra-query.sql](/docs/feature-extra-query.sql). Após executar as queries, execute o projeto com o seguinte comando



``` zsh
~$ dotnet run watch 
```

## Detalhes da implementação da API de Logging do .NET
