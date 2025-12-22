using System.ComponentModel.DataAnnotations;

namespace DomainModel;

public class Book : IEntity<int>
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; } = null!;

    public List<Author> Authors { get; set; } = [];

    public List<BookEdition> Editions { get; set; } = [];

    public List<Domain> Domains { get; set; } = [];
}