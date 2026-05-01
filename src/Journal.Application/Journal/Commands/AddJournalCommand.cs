using Journal.Application.DTOs;
using Journal.Domain.Abstractions;
using Journal.Application.Utils;
using MediatR;
using JournalEntity = Journal.Domain.Entities.Journal;

namespace Journal.Application.Journal.Commands
{
    public sealed class AddJournalCommand : IRequest<Result<JournalEntity>>
    {
        public string Title { get; set; }
        
        public string Content { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public Guid UserId { get; set; }

        public JournalEntity ToEntity()
        {
            return new JournalEntity
            {
                Name = Title,
                Issn = string.Empty,
                Qualisid = null,
                Aimscope = Content,
                Formatid = null,
                Apc = null,
                Url = string.Empty
            };
        }
    }
}