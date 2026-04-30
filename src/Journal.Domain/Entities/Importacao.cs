namespace Journal.Domain.Entities;

public partial class Importacao : Entity
{
    public string Issn { get; set; }

    public string Name { get; set; }

    public string Qualis2019 { get; set; }
}