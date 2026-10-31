# Journal Backend

ASP.NET Core 8 API for journal/qualis data. Run everything from the repo root unless stated.

## Commands

- Build: `dotnet build src/Journal.sln`
- Run API: `dotnet run --project src/Journal.Api` → http://localhost:5288, Swagger at `/swagger` (Development only), health at `/health`
  - The README's `cd src && dotnet run` does **not** work — there is no csproj in `src/`.
- No test project exists. `dotnet test` covers nothing; verify by building and calling endpoints (Swagger or `curl`).
- Local dependencies: MariaDB/MySQL is mandatory; Seq and RabbitMQ are optional.
  `cd src && docker compose up -d maria-db seq`
  `docker compose build api` is currently broken: `src/Dockerfile` still copies `Journal.Data/` and `Journal.MessageBus/` csproj files that no longer exist. Run `maria-db` + `seq` from compose and the API with `dotnet run`.

## Configuration

- Settings are flat top-level keys read via `configuration.GetSection("X").Value`: `ConnectionString`, `Seq`, `RabbitMQ`, `Redis`, `Jwt:Key`. Env vars override (compose passes `ConnectionString`, `RabbitMQ`, `Seq`, `Redis`).
- `src/Journal.Api/appsettings.Development.json` points at a machine-specific host (`note-samsung.local`); change it to localhost or set the `ConnectionString` env var.
- `Jwt:Key` is not committed. `POST /api/user/login` throws until it is set:
  `cd src/Journal.Api && dotnet user-secrets set Jwt:Key <32+ char secret>` (UserSecretsId is configured) or set `Jwt__Key`.
- Redis registration is commented out in `DependencyInjection.AddInfra`; leave it off unless Redis is actually running.

## Startup behavior

- `Program.Main` calls `DependencyInjection.ConfigureDatabase(...)` before `builder.Build()`: `EnsureDatabase` + DbUp scripts run on every startup. **The app crashes at launch if MySQL is unreachable.**
- Schema changes are DbUp scripts, not EF migrations — do not run `dotnet ef migrations`.
- Adding a script: create `src/Journal.Infrastructure/Persistence/Scripts/<n> - <name>.sql` **and** add a matching `<EmbeddedResource Include="...">` in `Journal.Infrastructure.csproj`; `.sql` files are not embedded automatically. Scripts apply in filename order, journaled by DbUp.
- Serilog writes to console, Seq (when `Seq` is set) and the MySQL `Logs` table (sink default; not created by the scripts). Quartz `CleanLogJob` deletes `Logs` rows older than 7 days every 12 hours.

## Architecture

- Layering: `Journal.Api` (controllers, Quartz job, RabbitMQ consumer) → `Journal.CrossCuting` (composition root, all DI) → `Journal.Application` (MediatR handlers, validators, DTOs, utils) → `Journal.Infrastructure` (EF Core/Pomelo, repositories, DbUp scripts, Serilog, JWT, RabbitMQ) → `Journal.Domain` (entities, repo interfaces in `Abstractions`/`Interfaces`).
- All registrations live in `src/Journal.CrossCuting/AppDependency/DependencyInjection.cs` (`AddInfra`, `AddCustomLog`, `ConfigureDatabase`). MediatR handlers and FluentValidation validators are auto-discovered from the `Journal.Application` assembly.
- Handler convention: return `Journal.Application.Utils.Result<T>`; inject `IValidator<T>` and call `ValidateAsync` manually (there is no validation pipeline behavior), map failures to `ErrorResponse`, persist via `IUnitOfWork`/`CommitAsync`. Controllers map `IsSuccess` → `Ok(result.Data)` / `BadRequest(result.Errors)`.
- Validators live beside commands in `Commands/Validations/`. Journal endpoints use DTOs; user endpoints bind commands directly from the request body.
- To add a use case: `Journal.Application/<Area>/{Commands|Queries}/X{Command,Query}.cs` + handler + `Validations/XCommandValidator.cs`, then a thin controller action calling `_mediator.Send`.
- Namespaces don't always match folders (e.g. `Journal.Application/Services/HealthCheckService.cs` declares `Journal.Infrastructure.Persistence.Services.MysqlDbHealthCheckService`; `CleanLogCommandHandler` is in namespace `Journal.Api.Jobs`). Search by symbol.
- Two `IPublisher` types coexist: MediatR's `IPublisher` and RabbitMQ's `Journal.Infrastructure.MessageBus.IPublisher<T>`. `JournalConsumer` is commented out in `Program.cs`, so `JournalQueue` messages are published but never consumed; RabbitMQ connections are opened per publish, so publishing without RabbitMQ running throws.
- EF Core handles repository reads/writes; raw Dapper through `DbServerData.OpenConnectionAsync` is used for ad-hoc SQL. Table/column names are lowercase and mapped in `Persistence/EntityConfiguration/*.Map.cs`; `Entity.Id`/`CreatedAt`/`UpdatedAt` have protected setters (DB-generated).
- Existing quirk: `PUT /api/journal/{id}` returns `BadRequest` on success and `Ok` on failure — don't copy that branch pattern.
