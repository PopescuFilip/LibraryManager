using DomainModel;
using FluentValidation.TestHelper;
using ServiceLayer.BookEditions;
using System.Diagnostics.CodeAnalysis;

namespace ServiceLayer.UnitTests;

[TestClass]
[ExcludeFromCodeCoverage]
public class BookEditionValidatorTests
{
    private BookEditionValidator _bookEditionValidator = default!;

    [TestInitialize]
    public void Init()
    {
        _bookEditionValidator = new BookEditionValidator();
    }

    [TestMethod]
    public void Validator_ShouldReturnIsValid_WhenNumberOfPagesIsZero()
    {
        var bookEdition = new BookEdition("name", 0, BookType.Paperback, 3);

        var result = _bookEditionValidator.TestValidate(bookEdition);

        result.ShouldHaveValidationErrorFor(x => x.PagesCount);
    }

    [TestMethod]
    public void Validator_ShouldReturnIsValid_WhenNumberOfPagesIsNegative()
    {
        var bookEdition = new BookEdition("name", -31, BookType.Paperback, 3);

        var result = _bookEditionValidator.TestValidate(bookEdition);

        result.ShouldHaveValidationErrorFor(x => x.PagesCount);
    }

    [TestMethod]
    public void Validator_ShouldReturnIsValid_WhenNameIsEmpty()
    {
        var bookEdition = new BookEdition(string.Empty, 21, BookType.Paperback, 3);

        var result = _bookEditionValidator.TestValidate(bookEdition);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [TestMethod]
    public void Validator_ShouldReturnIsValid_WhenBookEditionIsValid()
    {
        var bookEdition = new BookEdition("name", 21, BookType.Paperback, 3);

        var result = _bookEditionValidator.TestValidate(bookEdition);

        result.ShouldNotHaveAnyValidationErrors();
    }
}