# CostumerSolution API

Este é um projeto ASP.NET Core que implementa a arquitetura Clean Architecture e o padrão CQRS (Command Query Responsibility Segregation). O objetivo é fornecer uma API para gerenciar informações de clientes, utilizando boas práticas de desenvolvimento e separação de responsabilidades.

## Sumário

- [Pré-requisitos](#pré-requisitos)
- [Como rodar o projeto](#como-rodar-o-projeto)
  - [Rodando com Docker](#rodando-com-docker)
  - [Configurando o Banco de Dados](#configurando-o-banco-de-dados)
  - [Conexão com o SQL Server](#conexão-com-o-sql-server)
- [Clean Architecture e CQRS](#clean-architecture-e-cqrs)
- [Relacionamentos](#relacionamentos)
- [Resiliência](#resiliência)
- [Testes](#testes)
- [Validações](#validações)
- [Mapeamento de objetos](#mapeamento-de-objetos)
- [Docker](#docker)

## Pré-requisitos

Certifique-se de que você possui os seguintes pré-requisitos instalados em sua máquina:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/get-started) (opcional)

## Como rodar o projeto

### Rodando com Docker

1. **Construa o container do Docker**:

   No diretório do projeto, execute o seguinte comando:

   ```bash
   docker-compose up --build -d
   ```
   Esse comando criará o container com uma versão da API e com o banco de dados SQL Server caso queira usar.
   
### Configurando o Banco de Dados

No arquivo `Program.cs`, você pode escolher qual banco de dados utilizar:

```csharp
// Para usar o InMemory database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("CostumerDb"));

// Para usar o SQL Server
// builder.Services.AddDbContext<AppDbContext>(options =>
//     options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
```

Comente ou descomente as linhas acima para escolher entre o banco de dados em memória ou o SQL Server.

### Conexão com o SQL Server

Se você optar por usar o SQL Server, não se esqueça de configurar a string de conexão no arquivo `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost, 1433;Database=CostumerDb;User Id=sa;Password=Teste123*;"
  }
}
```

## Clean Architecture e CQRS

**Clean Architecture** é uma abordagem de design que visa criar sistemas de software que sejam independentes de frameworks, bancos de dados e interfaces de usuário. Ela promove a separação de preocupações e facilita a manutenção e testes do código. Esse foi o motivo pelo qual decidi usar essa arquitetura, inclusive optei por usar pastas ao inves de projetos separados pelo fato de ser um projeto pequeno e com poucos relacionamentos.

**CQRS** (Command Query Responsibility Segregation) é um padrão que separa a leitura e a escrita de dados. Em vez de usar uma única interface para comandos (escritas) e consultas (leituras), o CQRS sugere que você utilize diferentes modelos para cada um. Isso permite otimizar a performance e escalar a aplicação de forma mais eficiente, além de proporcionar uma melhor organização do código. Optei por usar essa abordagem, pois acredito ser uma ótima forma de separar as funcionalidades da web API.

## Relacionamentos

Devido ao fato de ter apenas uma entidade principal que é a do cliente, optei por usar value objects para criar os relacionamentos por não terem uma identidade única e principalmente para seguir o DDD (Domain-Driven Design), modelando o domínio de uma forma mais significativa e fácil de entender.

## Resiliência

Utilizar uma política de resiliência para a API Viacep não apenas melhora a confiabilidade e a robustez da aplicação, mas também oferece uma melhor experiência ao usuário e flexibilidade no gerenciamento de falhas. Utilizei o Circuit Breaker que ajuda a evitar que a aplicação continue fazendo solicitações a um serviço que está falhando repetidamente e o Retry que pode tentar novamente uma solicitação que falhou, seja por uma exceção ou por uma resposta de erro do servidor.

## Testes

Testes unitários são fundamentais para garantir a qualidade e a robustez de uma aplicação. Utilizei as bibliotecas Moq e FluentAssertions para auxiliar esse processo. Foquei em testar o Repository e os Use Cases, pois foram onde ficaram armazenadas toda a lógica da aplicação e chamadas com o banco de dados. 

## Validações

Utillizei a biblioteca FluentValidation que me permitiu criar regras de validação de maneira legível e expressiva. Além de complementar com a validação de CNPJ para aceitar apenas CNPJs existentes.

## Mapeamento de objetos

O uso do Automapper foi fundamental para evitar expor minha entidade desnecessáriamente e poder automatizar o processo de mapeamento, reconhecendo propriedades com nomes correspondentes entre os objetos de origem e destino.

## Docker

Tomei a iniciativa de criar o container no Docker para a praticidade no caso de testes na migration e na aplicação usando um banco de dados real.
