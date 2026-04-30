using Journal.Infrastructure.MessageBus.Queues;

namespace Journal.Application.DTOs
{
    public class JournalAddRequest
    {
        public string Issn { get; set; }
        public string Name { get; set; }
        public string Qualis { get; set; }

        public JournalMessage ToModel()
        {
            return new JournalMessage()
            {
                Issn = Issn,
                Name = Name,
                Qualis2019 = Qualis
            };
        }
    }
}