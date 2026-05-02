using MediatR;
using Journal.Application.Utils;

namespace Journal.Application.Journal.Commands
{
    public sealed class DeleteJournalCommand : IRequest<Result<bool>>
    {
        public Guid Id { get; set; }
    }
}