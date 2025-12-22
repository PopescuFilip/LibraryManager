using System.ComponentModel.DataAnnotations;

namespace DomainModel;

public class Domain : IEntity<int>
{
    public int Id { get; init; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; } = null!;

    public Domain? ParentDomain { get; init; }

    public List<Domain> SubDomains { get; init; } = [];

    public List<Book> Books { get; set; } = [];

    private Domain() {}

    public static Domain CreateNew(string name, Domain? parentDomain = null) =>
        new()
        {
            Name = name,
            ParentDomain = parentDomain
        };
}