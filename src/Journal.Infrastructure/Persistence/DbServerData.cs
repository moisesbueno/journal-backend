using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Data;

namespace Journal.Infrastructure.Persistence
{
    public class DbServerData(IConfiguration configuration)
    {
        private readonly IConfiguration _configuration = configuration;

        public async Task<IDbConnection> OpenConnectionAsync(CancellationToken cancellationToken = default)
        {
            var connection = new MySqlConnection(_configuration.GetSection("ConnectionString").Value);
            await connection.OpenAsync(cancellationToken);
            return connection;
        }
    }
}