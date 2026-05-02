namespace Journal.Domain.Entities;

public partial class Format : Entity
{
    public int? Maxpages { get; set; }

    public int? Maxwords { get; set; }

    public int? Space { get; set; }

    public int? Fontsize { get; set; }

    public virtual ICollection<Journal> Journals { get; set; } = new List<Journal>();
}