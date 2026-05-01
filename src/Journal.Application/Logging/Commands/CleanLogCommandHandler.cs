using Dapper;
using Journal.Infrastructure.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Journal.Api.Jobs
{
    public class CleanLogCommandHandler : IRequestHandler<CleanLogCommand, int>
    {
        private readonly DbServerData _dataSource;
        private readonly ILogger<CleanLogCommandHandler> _logger;

        public CleanLogCommandHandler(DbServerData dataSource, ILogger<CleanLogCommandHandler> logger)
        {
            _dataSource = dataSource;
            _logger = logger;
        }

        public async Task<int> Handle(CleanLogCommand request, CancellationToken cancellationToken)
        {
            using var connection = await _dataSource.OpenConnectionAsync();

            var timestamp = request.Timestamp ?? DateTime.UtcNow.AddDays(-7);
            var result = await connection.ExecuteAsync("DELETE FROM Logs WHERE Timestamp <= @Timestamp",
                                                        new { Timestamp = timestamp });

            _logger.LogInformation("Clean log table with result {result}", result);
            return result;
        }
    }
}