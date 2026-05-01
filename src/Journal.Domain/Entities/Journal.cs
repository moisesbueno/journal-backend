namespace Journal.Domain.Entities;

public partial class Journal : Entity
{
    public string Issn { get; set; }

    public string Name { get; set; }

    public Guid? Qualisid { get; set; }

    public string Aimscope { get; set; }

    public Guid? Formatid { get; set; }

    public bool? Apc { get; set; }

    public string Url { get; set; }

    public virtual Format Format { get; set; }

    public virtual Qualis Qualis { get; set; }

    public Journal()
    {
        Issn = string.Empty;
        Name = string.Empty;
        Qualisid = null;
        Aimscope = string.Empty;
        Formatid = null;
        Apc = null;
        Url = string.Empty;
    }
}