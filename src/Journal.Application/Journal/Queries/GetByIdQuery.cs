using Journal.Application.DTOs;
using MediatR;

namespace Journal.Application.Journal.Queries
{
    public class GetByIdQuery : IRequest<JournalDto>
    {
        public Guid Id { get; set; }
    }
}
