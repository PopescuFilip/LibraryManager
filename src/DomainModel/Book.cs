using System.ComponentModel.DataAnnotations;

namespace DomainModel;

public class Book
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; } = null!;

    public List<Author> Authors { get; set; } = [];

    public List<Edition> Editions { get; set; } = [];

    public List<Domain> Domains { get; set; } = [];
}