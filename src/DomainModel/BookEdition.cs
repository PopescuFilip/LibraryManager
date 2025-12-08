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

public class BookEdition : IEntity<int>
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; } = null!;

    [Required]
    [Range(1, 10_000)]
    public int PagesCount { get; set; }

    [Required]
    public BookType BookType { get; set; }

    [Required]
    public Book Book { get; set; } = null!;

    public List<BookRecord> BookRecords { get; set; } = [];
}