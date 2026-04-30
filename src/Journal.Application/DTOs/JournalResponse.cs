namespace Journal.Api.Models
{
    public class JournalResponse
    {
        public Guid Id { get; set; }
        public string Name { get; private set; }
        public string Issn { get; set; }
    }
}