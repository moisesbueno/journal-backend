namespace Journal.Api.Models
{
    public class JournalUpdateRequest
    {
        public string Title { get; set; }
        
        public string Content { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public Guid UserId { get; set; }
    }
}