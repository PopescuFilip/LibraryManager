using System.ComponentModel.DataAnnotations;

namespace DomainModel;

public enum BookType
{
    Hardcover,
    Paperback,
    SpiralBound,
    LargePrint,
    Braille,
    BoardBook
}

public class BookEdition : IEntity
{
    public int Id { get; init; }

    [Required]
    [StringLength(50)]
    public string Name { get; init; } = null!;

    [Required]
    [Range(1, 10_000)]
    public int PagesCount { get; init; }

    [Required]
    public BookType BookType { get; init; }

    [Required]
    public int BookDefinitionId { get; init; }

    public BookDefinition BookDefinition { get; private set; } = null!;

    public List<Book> BookRecords { get; } = [];
}