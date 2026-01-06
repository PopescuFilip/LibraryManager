using DomainModel;
using FluentValidation;
using NSubstitute;
using ServiceLayer.BookEditions;
using ServiceLayer.CRUD;
using System.Diagnostics.CodeAnalysis;

namespace ServiceLayer.UnitTests;

[TestClass]
[ExcludeFromCodeCoverage]
public class BookEditionServiceTests
{
    private BookEditionService _bookEditionService = default!;
    private IEntityService<BookEdition> _entityService = default!;
    private IEntityService<BookDefinition> _bookDefinitionService = default!;
    private IValidator<BookEdition> _validator = default!;
    private IValidator<BookAddOptions> _optionsValidator = default!;
    private IBookEditionQueryService _queryService = default!;

    [TestInitialize]
    public void Init()
    {
        _entityService = Substitute.For<IEntityService<BookEdition>>();
        _bookDefinitionService = Substitute.For<IEntityService<BookDefinition>>();
        _validator = Substitute.For<IValidator<BookEdition>>();
        _optionsValidator = Substitute.For<IValidator<BookAddOptions>>();
        _queryService = Substitute.For<IBookEditionQueryService>();
        _bookEditionService = new BookEditionService(
            _entityService,
            _bookDefinitionService,
            _validator,
            _optionsValidator,
            _queryService);
    }

    [TestMethod]
    public void Create_ShouldReturnInvalid_WhenBookDefinitionIsNotFound()
    {
        var name = "name";
        var pagesCount = 123;
        var bookType = BookType.Paperback;
        var definitionId = 32;
        _bookDefinitionService.GetById(definitionId).Returns((BookDefinition?)null);
        _entityService.Insert(Arg.Any<BookEdition>(), _validator)
            .Returns(call => Result.Valid(call.Arg<BookEdition>()));

        var result = _bookEditionService.Create(name, pagesCount, bookType, definitionId);

        Assert.IsFalse(result.IsValid);
    }

    [TestMethod]
    public void Create_ShouldReturnInvalid_WhenInsertReturnsInvalid()
    {
        var name = "name";
        var pagesCount = 123;
        var bookType = BookType.Paperback;
        var definitionId = 32;
        var existingBookDefinition = new BookDefinition("bookDefinition", [], [])
        {
            Id = definitionId
        };
        _bookDefinitionService.GetById(definitionId).Returns(existingBookDefinition);
        _entityService.Insert(Arg.Any<BookEdition>(), _validator)
            .Returns(Result.Invalid());

        var result = _bookEditionService.Create(name, pagesCount, bookType, definitionId);

        Assert.IsFalse(result.IsValid);
    }

    [TestMethod]
    public void Create_ShouldInsertAndReturnBookEdition_WhenAllValidationsPass()
    {
        var name = "name";
        var pagesCount = 123;
        var bookType = BookType.Paperback;
        var definitionId = 32;
        var existingBookDefinition = new BookDefinition("bookDefinition", [], [])
        {
            Id =  definitionId
        };
        _bookDefinitionService.GetById(definitionId).Returns(existingBookDefinition);
        _entityService.Insert(Arg.Any<BookEdition>(), _validator)
            .Returns(call => Result.Valid(call.Arg<BookEdition>()));

        var result = _bookEditionService.Create(name, pagesCount, bookType, definitionId);

        Assert.IsTrue(result.IsValid);
        var bookEdition = result.Get();
        Assert.AreEqual(name, bookEdition.Name);
        Assert.AreEqual(pagesCount, bookEdition.PagesCount);
        Assert.AreEqual(bookType, bookEdition.BookType);
        Assert.AreEqual(definitionId, bookEdition.BookDefinitionId);
    }
}