using System.ComponentModel.DataAnnotations;

namespace DomainModel;

public class Author : IEntity<int>
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; } = null!;

    public List<Book> Books { get; set; } = [];
}