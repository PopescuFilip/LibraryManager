namespace DomainModel;

[TestClass]
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
    }
}