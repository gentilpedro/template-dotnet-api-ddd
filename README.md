# GentilPedro.Templates.ApiDdd

Template `dotnet new` de Web API .NET 10 com arquitetura **DDD tática**.

## O que vem incluído

- **Domain rico**: agregado de exemplo (`Order`/`OrderItem`), Value Object (`Money`), eventos de domínio (`OrderCreatedDomainEvent`), interfaces de repositório dentro do Domain.
- **Application**: CQRS leve (Commands/Queries + Handlers, sem depender de MediatR), DTOs, mapeamentos AutoMapper, validação com FluentValidation.
- **Infrastructure**: EF Core + Npgsql, implementação dos repositórios, autenticação JWT + BCrypt.
- **API**: Controllers, middleware de tratamento de exceção, logging com Serilog (console + arquivo), documentação OpenAPI via Scalar.

## Como criar um projeto novo

**1. Configure o GitHub Packages como fonte NuGet** (uma vez só, nesta máquina — precisa de um token seu com escopo `read:packages`):

```bash
dotnet nuget add source https://nuget.pkg.github.com/gentilpedro/index.json -n github-gentilpedro -u gentilpedro -p <SEU_TOKEN_COM_read:packages> --store-password-in-clear-text
```

**2. Instale o template:**

```bash
dotnet new install GentilPedro.Templates.ApiDdd
```

**3. Crie o projeto:**

```bash
dotnet new api-ddd -n MeuApp
```

Configure a connection string do Postgres e o segredo do JWT em `appsettings.json`/`appsettings.Development.json` antes de rodar.

### Atualizar para a versão mais nova

```bash
dotnet new update
```

## Stack

AutoMapper · BCrypt.Net-Next · FluentValidation + FluentValidation.AspNetCore · Microsoft.AspNetCore.Authentication.JwtBearer · Microsoft.AspNetCore.OpenApi · Microsoft.EntityFrameworkCore (+ Design/Tools) · Npgsql.EntityFrameworkCore.PostgreSQL · Scalar.AspNetCore · Serilog.AspNetCore (+ Sinks Console/File)
