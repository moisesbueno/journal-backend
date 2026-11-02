using Journal.Application.DTOs;
using Journal.Domain.Abstractions;
using MediatR;

namespace Journal.Application.Journal.Queries
{
    public class GetByIdQueryHandler : IRequestHandler<GetByIdQuery, JournalDto>
    {
        private readonly IJournalRepository _journalRepository;
        private readonly JournalCache _journalCache;

        public GetByIdQueryHandler(IJournalRepository journalRepository, JournalCache journalCache)
        {
            _journalRepository = journalRepository;
            _journalCache = journalCache;
        }

        public async Task<JournalDto> Handle(GetByIdQuery request, CancellationToken cancellationToken)
        {
            var cached = await _journalCache.GetAsync(request.Id, cancellationToken);

            if (cached is not null)
                return cached;

            var journal = await _journalRepository.GetByIdAsync(request.Id);

            if (journal is null)
                return null;

            var journalDto = JournalDto.FromEntity(journal);

            await _journalCache.SetAsync(journalDto, cancellationToken);

            return journalDto;
        }
    }
}
