# Journal Backend

ASP.NET Core 8 API for journal/qualis data.

## Requirements
- .NET 8.0 SDK
- MariaDB or MySQL — **required** (DbUp migrations run at startup; the API won't start without a database)
- Redis — optional (enables the distributed cache; falls back to in-memory)
- Seq — optional (structured logs)
- RabbitMQ — optional (message publishing)

## Setup Instructions

1. Start the database:
   ```bash
   docker run -d --name some-mariadb -p 3306:3306 -e MARIADB_ROOT_PASSWORD=my-secret-pw mariadb:latest
   ```
   Or use compose for the database, Seq and Redis:
   ```bash
   cd src && docker compose up -d maria-db seq redis-stack
   ```

2. (Optional) Run Redis:
   ```bash
   docker run -d --name redis-stack -p 6379:6379 -p 8001:8001 redis/redis-stack:latest
   ```

3. Configure `src/Journal.Api/appsettings.Development.json` — see the example below.
   The committed file points at `note-samsung.local`; change it to `localhost`.

4. Set the JWT signing key (it is not committed and the API will not start without it):
   ```bash
   cd src/Journal.Api
   dotnet user-secrets set "Jwt:Key" "<32+ char secret>"
   ```
   Or set the `Jwt__Key` environment variable.

5. Run the API:
   ```bash
   dotnet run --project src/Journal.Api
   ```
   - API: http://localhost:5288
   - Swagger (Development only): http://localhost:5288/swagger
   - Health: http://localhost:5288/health

## appsettings.Development.json example

```json
{
  "ConnectionString": "Server=localhost;Port=3306;Database=journal;Uid=root;Pwd=my-secret-pw;",
  "Seq": "http://localhost:5341/",
  "RabbitMQ": "",
  "Redis": "",
  "Jwt": {
    "Key": "<32+ char secret>"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

| Key | Required | Environment variable | Notes |
| --- | --- | --- | --- |
| `ConnectionString` | yes | `ConnectionString` | MariaDB/MySQL connection string |
| `Jwt:Key` | yes | `Jwt__Key` | 32+ char signing secret; the API fails fast if missing |
| `Redis` | no | `Redis` | Empty = in-memory distributed cache |
| `Seq` | no | `Seq` | Empty = no Seq sink |
| `RabbitMQ` | no | `RabbitMQ` | Empty = no message bus connection |

## Endpoints
- `POST /api/user` — create a user, returns `{ "email": "..." }`
- `POST /api/user/login` — returns `{ "token": "..." }`
- `GET /health` — health checks (MySQL)
- `GET /swagger` — OpenAPI UI

Journal routes (`/api/journal/*`) require authentication. Send the login token in the header:

```bash
curl -H "Authorization: Bearer <token>" http://localhost:5288/api/journal
```

Requests without a valid token return `401`. Swagger exposes an **Authorize** button for the same purpose.

## Notes
- Schema changes are DbUp scripts in `src/Journal.Infrastructure/Persistence/Scripts`; do not use `dotnet ef migrations`.
- The API fails fast at startup if MySQL is unreachable.
- `docker compose build api` is currently broken (`src/Dockerfile` references removed projects); run the API with `dotnet run`.
- There is no test project — verify changes with `dotnet build src/Journal.sln` and by calling the endpoints.

## Features Implemented
- User creation with bcrypt password hashing and duplicate-email validation
- Emails stored as deterministic HMAC-SHA256 hashes (case-insensitive uniqueness)
- User login with JWT authentication
- Database schema creation and seeding via DbUp
- `IMemoryCache` plus optional Redis-backed `IDistributedCache`
