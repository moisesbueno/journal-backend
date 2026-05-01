using Journal.Application.Utils;
using MediatR;
using JournalEntity = Journal.Domain.Entities.Journal;

namespace Journal.Application.Journal.Commands
{
    public sealed class AddJournalCommand : IRequest<Result<JournalEntity>>
    {
        public string Name { get; set; }
        
        public string Issn { get; set; }
        
        public JournalEntity ToEntity()
        {
            return new JournalEntity
            {
                Name = Name,
                Issn = Issn,
                Qualisid = null,
            };
        }
    }
}