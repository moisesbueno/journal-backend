using Journal.Application.Utils;
using Journal.Domain.Abstractions;
using MediatR;

using JournalEntity = Journal.Domain.Entities.Journal;

namespace Journal.Application.Journal.Queries
{
    public class GetJournalsQueryHandler : IRequestHandler<GetJournalsQuery, PaginatedList<JournalEntity>>
    {
        private readonly IJournalRepository _journalRepository;

        public GetJournalsQueryHandler(IJournalRepository journalRepository)
        {
            _journalRepository = journalRepository;
        }

        public async Task<PaginatedList<JournalEntity>> Handle(GetJournalsQuery request, CancellationToken cancellationToken)
        {
            var total = await _journalRepository.CountAsync(request.Search ?? "");

            var result = await _journalRepository.GetAsync(request.Search ?? "", request.PageNumber, request.PageSize);

            return new PaginatedList<JournalEntity>(result.ToList(), request.PageSize, request.PageNumber, total);
        }
    }
}
