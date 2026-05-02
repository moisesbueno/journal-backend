# Journal Backend

## Requirements
- .NET 8.0 SDK
- MySQL database
- Redis server

## Setup Instructions
1. Run MySQL database:
   ```bash
   docker run -d --name some-mariadb -p 3306:3306 -e MARIADB_ROOT_PASSWORD=root mariadb:latest
   ```

2. Run Redis server:
   ```bash
   docker run -d --name redis-stack -p 6379:6379 -p 8001:8001 redis/redis-stack:latest
   ```

3. Update connection strings in `appsettings.Development.json`:
   ```json
   {
     "ConnectionString": "Server=localhost;Database=journal;Uid=root;Pwd=root;"
   }
   ```

4. Run the application:
   ```bash
   cd src
   dotnet run
   ```

## Features Implemented
- User creation with password hashing
- User login with JWT authentication
- Database table creation via DbUp migrations
