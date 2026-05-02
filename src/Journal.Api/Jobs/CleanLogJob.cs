using Dapper;
using Journal.Infrastructure.Persistence;
using MediatR;
using Quartz;

namespace Journal.Api.Jobs
{
    public class CleanLogJob : IJob
    {
        private readonly DbServerData _dataSource;
        private readonly ILogger<CleanLogJob> _logger;
        private readonly IMediator _mediator;

        public CleanLogJob(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var cleanLogCommand = new CleanLogCommand()
            {
                Timestamp = DateTime.UtcNow.AddDays(-7)
            };
            await _mediator.Send(cleanLogCommand);
        }
    }
}