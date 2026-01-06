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

    public IEnumerable<Book> BookRecords => _books;

    private readonly List<Book> _books = [];

    public BookEdition(string name, int pagesCount, BookType bookType, int bookDefinitionId) =>
        (Name, PagesCount, BookType, BookDefinitionId) =
        (name, pagesCount, bookType, bookDefinitionId);

    public void AddBooks(BookStatus bookStatus, int count)
    {
        _books.AddRange(GetBooks(bookStatus, Id, count));
    }

    private static IEnumerable<Book> GetBooks(BookStatus bookStatus, int bookEditionId, int count) =>
        Enumerable.Range(0, count).Select(_ => new Book(bookStatus, bookEditionId));
}