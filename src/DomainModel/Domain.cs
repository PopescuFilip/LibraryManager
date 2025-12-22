using System.ComponentModel.DataAnnotations;

namespace DomainModel;

public class Domain : IEntity<int>
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; } = null!;

    public Domain? ParentDomain { get; set; }

    public List<Domain> SubDomains { get; set; } = [];

    public List<Book> Books { get; set; } = [];

    private Domain() {}

    public static Domain CreateNew(string name, Domain? parentDomain) =>
        new()
        {
            Name = name,
            ParentDomain = parentDomain
        };
}