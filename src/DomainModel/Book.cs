using System.ComponentModel.DataAnnotations;

namespace DomainModel;

public class Book : IEntity<int>
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; init; } = null!;

    public List<Author> Authors { get; } = [];

    public List<BookEdition> Editions { get; } = [];

    public List<Domain> Domains { get; } = [];

    public IEnumerable<Domain> ImplicitDomains => Domains;
}