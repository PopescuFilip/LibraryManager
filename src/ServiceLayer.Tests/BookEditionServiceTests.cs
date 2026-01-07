using DomainModel;
using FluentValidation;
using Microsoft.Extensions.Options;
using NSubstitute;
using ServiceLayer.BookEditions;
using ServiceLayer.CRUD;
using ServiceLayer.UnitTests.TestHelpers;
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
    private IValidator<BooksUpdateOptions> _optionsValidator = default!;
    private IBookEditionQueryService _queryService = default!;

    [TestInitialize]
    public void Init()
    {
        _entityService = Substitute.For<IEntityService<BookEdition>>();
        _bookDefinitionService = Substitute.For<IEntityService<BookDefinition>>();
        _validator = Substitute.For<IValidator<BookEdition>>();
        _optionsValidator = Substitute.For<IValidator<BooksUpdateOptions>>();
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

    [TestMethod]
    public void AddBooks_ShouldReturnInvalid_WhenBookEditionIsNotFound()
    {
        var options = new BooksUpdateOptions(20, 14, 1);
        _optionsValidator.Validate(options).Returns(Validation.ValidResult);
        _queryService.GetByIdWithBooks(options.BookEditionId).Returns((BookEdition?)null);
        _entityService.Update(Arg.Any<BookEdition>(), _validator)
            .Returns(call => Result.Valid(call.Arg<BookEdition>()));

        var result = _bookEditionService.AddBooks(options);

        Assert.IsFalse(result.IsValid);
    }

    [TestMethod]
    public void AddBooks_ShouldReturnInvalid_WhenOptionsValidationFails()
    {
        var options = new BooksUpdateOptions(20, 14, 1);
        var bookEdition = new BookEdition("nameHere", 321, BookType.LargePrint, options.BookEditionId);
        _optionsValidator.Validate(options).Returns(Validation.InvalidResult);
        _queryService.GetByIdWithBooks(options.BookEditionId).Returns(bookEdition);
        _entityService.Update(bookEdition, _validator)
            .Returns(call => Result.Valid(call.Arg<BookEdition>()));

        var result = _bookEditionService.AddBooks(options);

        Assert.IsFalse(result.IsValid);
    }

    [TestMethod]
    public void AddBooks_ShouldUpdateBookEdition_WhenBooksAreAddedSuccessfully()
    {
        var options = new BooksUpdateOptions(20, 14, 1);
        var bookEdition = new BookEdition("nameHere", 321, BookType.LargePrint, options.BookEditionId);
        _optionsValidator.Validate(options).Returns(Validation.ValidResult);
        _queryService.GetByIdWithBooks(options.BookEditionId).Returns(bookEdition);
        _entityService.Update(bookEdition, _validator)
            .Returns(call => Result.Valid(call.Arg<BookEdition>()));

        var result = _bookEditionService.AddBooks(options);

        Assert.IsTrue(result.IsValid);
        var updatedBookEdition = result.Get();
        Assert.IsTrue(updatedBookEdition.BookRecords.MatchesPerfectly(options));
    }

    [TestMethod]
    public void AddBooks_ShouldReturnBookEdition_WhenBooksAreAddedSuccessfully()
    {
        var options = new BooksUpdateOptions(20, 14, 1);
        var bookEdition = new BookEdition("nameHere", 321, BookType.LargePrint, options.BookEditionId);
        _optionsValidator.Validate(options).Returns(Validation.ValidResult);
        _queryService.GetByIdWithBooks(options.BookEditionId).Returns(bookEdition);
        _entityService.Update(bookEdition, _validator)
            .Returns(call => Result.Valid(call.Arg<BookEdition>()));

        var result = _bookEditionService.AddBooks(options);

        Assert.IsTrue(result.IsValid);
    }

    [TestMethod]
    public void RemoveBooks_ShouldNotChangeBookEdition_WhenBooksCannotBeRemoved()
    {
        var removeOptions = new BooksUpdateOptions(20, 14, 1);
        var bookEdition = new BookEdition("nameHere", 321, BookType.LargePrint, removeOptions.BookEditionId);
        var addOptions = removeOptions with
        {
            ForBorrowingCount = removeOptions.ForReadingRoomCount,
            ForReadingRoomCount = removeOptions.ForReadingRoomCount - 3,
        };
        bookEdition.AddBooks(addOptions.ToStatusCountDictionary());
        _optionsValidator.Validate(removeOptions).Returns(Validation.ValidResult);
        _queryService.GetByIdWithBooks(removeOptions.BookEditionId).Returns(bookEdition);
        _entityService.Update(bookEdition, _validator)
            .Returns(call => Result.Valid(call.Arg<BookEdition>()));

        var result = _bookEditionService.RemoveBooks(removeOptions);

        Assert.IsFalse(result.IsValid);
        Assert.IsTrue(bookEdition.BookRecords.MatchesPerfectly(addOptions));
        _entityService.DidNotReceiveWithAnyArgs().Update(default!, default!);
    }

    [TestMethod]
    public void RemoveBooks_ShouldReturnInvalid_WhenBooksCannotBeRemoved()
    {
        var removeOptions = new BooksUpdateOptions(20, 14, 1);
        var bookEdition = new BookEdition("nameHere", 321, BookType.LargePrint, removeOptions.BookEditionId);
        var addOptions = removeOptions with
        {
            ForBorrowingCount = removeOptions.ForReadingRoomCount,
            ForReadingRoomCount = removeOptions.ForReadingRoomCount - 3,
        };
        bookEdition.AddBooks(addOptions.ToStatusCountDictionary());
        _optionsValidator.Validate(removeOptions).Returns(Validation.ValidResult);
        _queryService.GetByIdWithBooks(removeOptions.BookEditionId).Returns(bookEdition);
        _entityService.Update(bookEdition, _validator)
            .Returns(call => Result.Valid(call.Arg<BookEdition>()));

        var result = _bookEditionService.RemoveBooks(removeOptions);

        Assert.IsFalse(result.IsValid);
    }

    [TestMethod]
    public void RemoveBooks_ShouldReturnInvalid_WhenBookEditionIsNotFound()
    {
        var options = new BooksUpdateOptions(20, 14, 1);
        _optionsValidator.Validate(options).Returns(Validation.ValidResult);
        _queryService.GetByIdWithBooks(options.BookEditionId).Returns((BookEdition?)null);
        _entityService.Update(Arg.Any<BookEdition>(), _validator)
            .Returns(call => Result.Valid(call.Arg<BookEdition>()));

        var result = _bookEditionService.RemoveBooks(options);

        Assert.IsFalse(result.IsValid);
    }

    [TestMethod]
    public void RemoveBooks_ShouldReturnInvalid_WhenOptionsValidationFails()
    {
        var options = new BooksUpdateOptions(20, 14, 1);
        var bookEdition = new BookEdition("nameHere", 321, BookType.LargePrint, options.BookEditionId);
        bookEdition.AddBooks(options.ToStatusCountDictionary());
        _optionsValidator.Validate(options).Returns(Validation.InvalidResult);
        _queryService.GetByIdWithBooks(options.BookEditionId).Returns(bookEdition);
        _entityService.Update(bookEdition, _validator)
            .Returns(call => Result.Valid(call.Arg<BookEdition>()));

        var result = _bookEditionService.RemoveBooks(options);

        Assert.IsFalse(result.IsValid);
    }

    [TestMethod]
    public void RemoveBooks_ShouldUpdateBookEdition_WhenBooksAreRemovedSuccessfully()
    {
        var removeOptions = new BooksUpdateOptions(20, 14, 1);
        var bookEdition = new BookEdition("nameHere", 321, BookType.LargePrint, removeOptions.BookEditionId);
        var addOptions = removeOptions with
        {
            ForBorrowingCount = removeOptions.ForReadingRoomCount + 10,
            ForReadingRoomCount = removeOptions.ForReadingRoomCount + 3,
        };
        bookEdition.AddBooks(addOptions.ToStatusCountDictionary());
        _optionsValidator.Validate(removeOptions).Returns(Validation.ValidResult);
        _queryService.GetByIdWithBooks(removeOptions.BookEditionId).Returns(bookEdition);
        _entityService.Update(bookEdition, _validator)
            .Returns(call => Result.Valid(call.Arg<BookEdition>()));

        var result = _bookEditionService.RemoveBooks(removeOptions);

        Assert.IsTrue(result.IsValid);
        var updatedBookEdition = result.Get();
        var resultingOptions = addOptions.Substract(removeOptions);
        Assert.IsTrue(bookEdition.BookRecords.MatchesPerfectly(resultingOptions));
    }

    [TestMethod]
    public void RemoveBooks_ShouldReturnBookEdition_WhenBooksAreRemovedSuccessfully()
    {
        var options = new BooksUpdateOptions(20, 14, 1);
        var bookEdition = new BookEdition("nameHere", 321, BookType.LargePrint, options.BookEditionId);
        bookEdition.AddBooks(options.ToStatusCountDictionary());
        _optionsValidator.Validate(options).Returns(Validation.ValidResult);
        _queryService.GetByIdWithBooks(options.BookEditionId).Returns(bookEdition);
        _entityService.Update(bookEdition, _validator)
            .Returns(call => Result.Valid(call.Arg<BookEdition>()));

        var result = _bookEditionService.RemoveBooks(options);

        Assert.IsTrue(result.IsValid);
    }
}