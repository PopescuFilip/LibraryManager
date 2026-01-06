using System.Diagnostics.CodeAnalysis;

namespace DomainModel.UnitTests;

[TestClass]
[ExcludeFromCodeCoverage]
public class BookEditionTests
{
    [TestMethod]
    public void Constructor_ShouldWork()
    {
        var name = "name";
        var pagesCount = 12;
        var bookType = BookType.SpiralBound;
        var bookDefinitionId = 2;

        var bookEdition = new BookEdition(name, pagesCount, bookType, bookDefinitionId);

        Assert.AreEqual(name, bookEdition.Name);
        Assert.AreEqual(pagesCount, bookEdition.PagesCount);
        Assert.AreEqual(bookType, bookEdition.BookType);
        Assert.AreEqual(bookDefinitionId, bookEdition.BookDefinitionId);
        Assert.IsFalse(bookEdition.BookRecords.Any());
    }

    [TestMethod]
    public void AddBooks_ShouldDoNothingWhenGivenCountZeroOrLess()
    {
        var name = "name";
        var pagesCount = 12;
        var bookType = BookType.SpiralBound;
        var bookDefinitionId = 2;
        var bookEdition = new BookEdition(name, pagesCount, bookType, bookDefinitionId);

        bookEdition.AddBooks(BookStatus.ForReadingRoom, 0);
        bookEdition.AddBooks(BookStatus.ForReadingRoom, -1);

        Assert.IsFalse(bookEdition.BookRecords.Any());
    }

    [TestMethod]
    public void AddBooks_ShouldAddCorrectAmountOfCorrectBooks()
    {
        var name = "name";
        var pagesCount = 12;
        var bookType = BookType.SpiralBound;
        var bookDefinitionId = 2;
        var bookEdition = new BookEdition(name, pagesCount, bookType, bookDefinitionId);
        var forReadingRoomCount = 21;
        var availableCount = 34;

        bookEdition.AddBooks(BookStatus.ForReadingRoom, forReadingRoomCount);
        bookEdition.AddBooks(BookStatus.Available, availableCount);

        var dict = new Dictionary<BookStatus, int>()
        {
            { BookStatus.ForReadingRoom, forReadingRoomCount },
            { BookStatus.Available, availableCount }
        };
        Assert.IsTrue(bookEdition.BookRecords.MatchesPerfectly(dict));
    }
}