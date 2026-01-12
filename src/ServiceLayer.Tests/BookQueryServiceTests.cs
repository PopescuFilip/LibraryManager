using DomainModel;
using DomainModel.DTOs;
using ServiceLayer.CRUD;
using ServiceLayer.UnitTests.TestHelpers;
using System.Diagnostics.CodeAnalysis;

namespace ServiceLayer.UnitTests;

[TestClass]
[ExcludeFromCodeCoverage]
public class BookQueryServiceTests
{
    private BookQueryService _bookQueryService = default;
    private FakeRepository<Book> _fakeRepository = default;

    [TestInitialize]
    public void Init()
    {
        _fakeRepository = new FakeRepository<Book>();
        _bookQueryService = new BookQueryService(_fakeRepository);
    }

    [TestMethod]
    public void GetBookDetails_ShouldReturnCorrectBookDetails()
    {
        var allIds = Enumerable.Range(1, 100);
        var interestingIds = allIds.Take(10).ToIdCollection();
        var books = allIds.Select(id => new Book(BookStatus.Available, id)
        {
            Id = id,
            BookEdition = Create(id)
        }).ToList();
        _fakeRepository.SetSourceValues(books);

        var bookDetails = _bookQueryService.GetBookDetails(interestingIds);

        Assert.AreEqual(interestingIds.Count, bookDetails.Count);
        var actualIds = bookDetails.Select(x => x.BookId);
        Assert.IsTrue(interestingIds.All(actualIds.Contains));
    }

    private static BookEdition Create(int id)
    {
        var bookEdition = new BookEdition("name", id, BookType.LargePrint, id)
        {
            BookDefinition = new BookDefinition("name", [new Author("ads")], [new Domain("")])
        };

        var bookAddOptions = new Dictionary<BookStatus, int>()
        {
            { BookStatus.Borrowed, id },
            { BookStatus.Available, id },
            { BookStatus.ForReadingRoom, id },
        };
        bookEdition.AddBooks(bookAddOptions);
        return bookEdition;
    }
}
