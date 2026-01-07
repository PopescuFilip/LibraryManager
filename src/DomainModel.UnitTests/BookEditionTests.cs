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
    public void AddBooks_ShouldAddCorrectAmountOfCorrectBooks_WhenCallingItMultipleTimes()
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

    [TestMethod]
    public void AddBooks_ShouldAddCorrectAmountOfCorrectBooks()
    {
        var name = "name";
        var pagesCount = 12;
        var bookType = BookType.SpiralBound;
        var bookDefinitionId = 2;
        var bookEdition = new BookEdition(name, pagesCount, bookType, bookDefinitionId);
        var forReadingRoomCount = 221;

        bookEdition.AddBooks(BookStatus.ForReadingRoom, forReadingRoomCount);

        Assert.AreEqual(forReadingRoomCount, bookEdition.BookRecords.Count());
        Assert.IsTrue(bookEdition.BookRecords.All(x => x.Status == BookStatus.ForReadingRoom));
    }

    [TestMethod]
    public void AddBooks_ShouldAddCorrectAmountOfCorrectBooks_WhenUsingDictionaryVersion()
    {
        var name = "name";
        var pagesCount = 12;
        var bookType = BookType.SpiralBound;
        var bookDefinitionId = 2;
        var bookEdition = new BookEdition(name, pagesCount, bookType, bookDefinitionId);
        var dict = new Dictionary<BookStatus, int>()
        {
            { BookStatus.ForReadingRoom, 54 },
            { BookStatus.Available, 31 }
        };

        bookEdition.AddBooks(dict);

        Assert.IsTrue(bookEdition.BookRecords.MatchesPerfectly(dict));
    }

    [TestMethod]
    public void TryRemoveBooks_ShouldNotAffectObject_WhenItFails()
    {
        var name = "name";
        var pagesCount = 12;
        var bookType = BookType.SpiralBound;
        var bookDefinitionId = 2;
        var bookEdition = new BookEdition(name, pagesCount, bookType, bookDefinitionId);
        var initial = new Dictionary<BookStatus, int>()
        {
            { BookStatus.ForReadingRoom, 54 },
            { BookStatus.Available, 31 }
        };
        bookEdition.AddBooks(initial);
        var toRemove = new Dictionary<BookStatus, int>()
        {
            { BookStatus.ForReadingRoom, 5 },
            { BookStatus.Available, 33 }
        };

        var success = bookEdition.TryRemoveBooks(toRemove);

        Assert.IsFalse(success);
        Assert.IsTrue(bookEdition.BookRecords.MatchesPerfectly(initial));
    }

    [TestMethod]
    public void TryRemoveBooks_ShouldFail_WhenBooksCannotBeRemoved()
    {
        var name = "name";
        var pagesCount = 12;
        var bookType = BookType.SpiralBound;
        var bookDefinitionId = 2;
        var bookEdition = new BookEdition(name, pagesCount, bookType, bookDefinitionId);
        var initial = new Dictionary<BookStatus, int>()
        {
            { BookStatus.ForReadingRoom, 54 },
            { BookStatus.Available, 31 }
        };
        bookEdition.AddBooks(initial);
        var toRemove = new Dictionary<BookStatus, int>()
        {
            { BookStatus.ForReadingRoom, 663 },
            { BookStatus.Available, 2 }
        };

        var success = bookEdition.TryRemoveBooks(toRemove);

        Assert.IsFalse(success);
    }

    [TestMethod]
    public void TryRemoveBooks_ShouldRemoveCorrectAmountOfCorrectBooks_WhenAStatusWillNotHaveRemainingBooks()
    {
        var name = "name";
        var pagesCount = 12;
        var bookType = BookType.SpiralBound;
        var bookDefinitionId = 2;
        var bookEdition = new BookEdition(name, pagesCount, bookType, bookDefinitionId);
        var initial = new Dictionary<BookStatus, int>()
        {
            { BookStatus.ForReadingRoom, 54 },
            { BookStatus.Available, 31 }
        };
        bookEdition.AddBooks(initial);
        var toRemove = new Dictionary<BookStatus, int>()
        {
            { BookStatus.ForReadingRoom, 54 },
            { BookStatus.Available, 2 }
        };

        var success = bookEdition.TryRemoveBooks(toRemove);

        Assert.IsTrue(success);
        Assert.IsTrue(bookEdition.BookRecords.MatchesPerfectly(initial.Substract(toRemove)));
        Assert.IsFalse(bookEdition.BookRecords.Any(x => x.Status == BookStatus.ForReadingRoom));
    }

    [TestMethod]
    public void TryRemoveBooks_ShouldRemoveCorrectAmountOfCorrectBooks_WhenBooksCanBeRemoved()
    {
        var name = "name";
        var pagesCount = 12;
        var bookType = BookType.SpiralBound;
        var bookDefinitionId = 2;
        var bookEdition = new BookEdition(name, pagesCount, bookType, bookDefinitionId);
        var initial = new Dictionary<BookStatus, int>()
        {
            { BookStatus.ForReadingRoom, 54 },
            { BookStatus.Available, 31 }
        };
        bookEdition.AddBooks(initial);
        var toRemove = new Dictionary<BookStatus, int>()
        {
            { BookStatus.ForReadingRoom, 44 },
            { BookStatus.Available, 2 }
        };

        var success = bookEdition.TryRemoveBooks(toRemove);

        Assert.IsTrue(success);
        Assert.IsTrue(bookEdition.BookRecords.MatchesPerfectly(initial.Substract(toRemove)));
    }
}