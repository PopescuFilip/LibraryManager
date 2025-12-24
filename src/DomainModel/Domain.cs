using System.ComponentModel.DataAnnotations;

namespace DomainModel;

public class Domain : IEntity<int>
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; init; } = null!;

    public int? ParentDomainId { get; init; }

    public Domain? ParentDomain { get; init; }

    public List<Domain> SubDomains { get; } = [];

    public List<Book> Books { get; } = [];

    private Domain() {}

    public static Domain CreateNew(string name, int? parentDomainId = null) =>
        new()
        {
            Name = name,
            ParentDomainId = parentDomainId
        };
}