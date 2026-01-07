using DomainModel;
using ServiceLayer.CRUD;
using ServiceLayer.UnitTests.TestHelpers;
using System.Diagnostics.CodeAnalysis;

namespace ServiceLayer.UnitTests;

[TestClass]
[ExcludeFromCodeCoverage]
public class BookEditionQueryServiceTests
{
    private BookEditionQueryService _queryService = default!;
    private FakeRepository<BookEdition> _repository = default!;

    [TestInitialize]
    public void Init()
    {
        _repository = new FakeRepository<BookEdition>();
        _queryService = new BookEditionQueryService(_repository);
    }

    [TestMethod]
    public void GetByIdWithBooks_ShouldReturnNull_WhenEditionWithIdDoesNotExist()
    {
        var interestingId = 321;
        var ids = new List<int>() { 1, 4, 54, 433, 54 };
        var bookEditions = Generator.GenerateBookEditionsFrom(ids);
        _repository.SetSourceValues(bookEditions);

        var bookEdition = _queryService.GetByIdWithBooks(interestingId);

        Assert.IsNull(bookEdition);
    }

    [TestMethod]
    public void GetByIdWithBooks_ShouldReturnCorrectBookEdition_WhenItExists()
    {
        var interestingId = 321;
        var ids = new List<int>() { 1, 4, 54, 433, interestingId, 54 };
        var bookEditions = Generator.GenerateBookEditionsFrom(ids);
        _repository.SetSourceValues(bookEditions);

        var bookEdition = _queryService.GetByIdWithBooks(interestingId);

        Assert.IsNotNull(bookEdition);
        Assert.AreEqual(interestingId, bookEdition.Id);
    }
}