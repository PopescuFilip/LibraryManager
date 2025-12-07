using System.ComponentModel.DataAnnotations;

namespace DomainModel;

public class Domain
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; } = null!;

    public Domain? ParentDomain { get; set; }

    public List<Domain> SubDomains { get; set; } = [];

    public List<Book> Books { get; set; } = [];
}