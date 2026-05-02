using Journal.Application.DTOs;
using Journal.Domain.Abstractions;
using MediatR;
using JournalEntity = Journal.Domain.Entities.Journal;
using Journal.Application.Utils;

namespace Journal.Application.Journal.Commands
{
    public sealed class UpdateJournalCommand : IRequest<Result<JournalEntity>>
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }
        
        public string Aimscope { get; set; }
        
        public JournalEntity ToEntity()
        {
            return new JournalEntity(Id,Name,Aimscope);
        }
    }
}