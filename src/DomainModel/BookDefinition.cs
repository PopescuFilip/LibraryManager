using System.ComponentModel.DataAnnotations;

namespace DomainModel;

public class BookDefinition : IEntity<int>
{
    public int Id { get; init; }

    [Required]
    [StringLength(50)]
    public string Name { get; init; } = null!;

    public List<Author> Authors { get; } = [];

    public List<BookEdition> Editions { get; } = [];

    public List<Domain> Domains { get; } = [];

    public IEnumerable<Domain> ImplicitDomains => Domains;

    // used by EF
    private BookDefinition() { }

    public BookDefinition(string name, IEnumerable<Author> authors, IEnumerable<Domain> domains) =>
        (Name, Authors, Domains) = (name, authors.ToList(), domains.ToList());
}