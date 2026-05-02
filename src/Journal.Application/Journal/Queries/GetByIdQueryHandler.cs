using Journal.Application.Utils;
using Journal.Domain.Abstractions;
using MediatR;

using JournalEntity = Journal.Domain.Entities.Journal;

namespace Journal.Application.Journal.Queries
{
    public class GetByIdQueryHandler : IRequestHandler<GetByIdQuery, JournalEntity?>
    {
        private readonly IJournalRepository _journalRepository;

        public GetByIdQueryHandler(IJournalRepository journalRepository)
        {
            _journalRepository = journalRepository;
        }

        public async Task<JournalEntity?> Handle(GetByIdQuery request, CancellationToken cancellationToken)
        {
            return await _journalRepository.GetByIdAsync(request.Id);
        }
    }
}