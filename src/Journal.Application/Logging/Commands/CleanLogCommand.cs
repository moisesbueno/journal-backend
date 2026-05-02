using MediatR;

namespace Journal.Api.Jobs
{
    public class CleanLogCommand : IRequest<int>
    {
        public DateTime? Timestamp { get; set; }
    }
}