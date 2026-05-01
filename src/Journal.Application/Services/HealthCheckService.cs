using Journal.Infrastructure.Persistence.Context;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Journal.Infrastructure.Persistence.Services;

public class MysqlDbHealthCheckService : IHealthCheck
{
    private readonly JournalContext _journalContext;

    public MysqlDbHealthCheckService(JournalContext journalContext)
    {
        _journalContext = journalContext;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            await _journalContext.Database.CanConnectAsync();
            return HealthCheckResult.Healthy();

        }
        catch
        {
            return HealthCheckResult.Unhealthy();
        }
    }
}