using System.ComponentModel.DataAnnotations;

namespace DomainModel;

public class Author : IEntity<int>
{
    public int Id { get; init; }

    [Required]
    [StringLength(50)]
    public string Name { get; init; } = null!;

    public List<BookDefinition> WrittenBooks { get; } = [];

    public Author(string name) => Name = name;
}