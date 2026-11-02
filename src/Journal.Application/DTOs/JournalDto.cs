using JournalEntity = Journal.Domain.Entities.Journal;

namespace Journal.Application.DTOs
{
    public class JournalDto
    {
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string Issn { get; set; }

        public string Name { get; set; }

        public Guid? Qualisid { get; set; }

        public string Aimscope { get; set; }

        public Guid? Formatid { get; set; }

        public bool? Apc { get; set; }

        public string Url { get; set; }

        public static JournalDto FromEntity(JournalEntity entity)
        {
            return new JournalDto
            {
                Id = entity.Id,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                Issn = entity.Issn,
                Name = entity.Name,
                Qualisid = entity.Qualisid,
                Aimscope = entity.Aimscope,
                Formatid = entity.Formatid,
                Apc = entity.Apc,
                Url = entity.Url,
            };
        }
    }
}
