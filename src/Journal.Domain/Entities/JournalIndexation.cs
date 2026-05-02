namespace Journal.Domain.Entities;

public partial class JournalIndexation : Entity
{
    public Guid? Journalid { get; set; }

    public Guid Journalindexationid { get; set; }

    public virtual Journal Journal { get; set; }

    public virtual DatabaseIndexation Journalindexation { get; set; }
}