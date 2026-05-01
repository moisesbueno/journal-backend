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