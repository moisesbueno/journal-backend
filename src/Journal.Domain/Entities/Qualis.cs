namespace Journal.Domain.Entities;

public partial class Qualis : Entity
{
    public string Description { get; set; }

    public virtual ICollection<Journal> Journals { get; set; } = new List<Journal>();
}