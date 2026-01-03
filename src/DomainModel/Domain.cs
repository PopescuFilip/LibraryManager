using System.ComponentModel.DataAnnotations;

namespace DomainModel;

public class Domain : IEntity
{
    public int Id { get; init; }

    [Required]
    [StringLength(50)]
    public string Name { get; init; } = null!;

    public int? ParentDomainId { get; init; }

    public Domain? ParentDomain { get; private set; }

    public List<Domain> SubDomains { get; } = [];

    public Domain(string name, int? parentDomainId = null) =>
        (Name, ParentDomainId) = (name, parentDomainId);
}