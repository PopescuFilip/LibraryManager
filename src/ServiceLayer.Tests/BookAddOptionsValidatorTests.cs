using FluentValidation.TestHelper;
using ServiceLayer.BookEditions;
using System.Diagnostics.CodeAnalysis;

namespace ServiceLayer.UnitTests;

[TestClass]
[ExcludeFromCodeCoverage]
public class BookAddOptionsValidatorTests
{
    private BookAddOptionsValidator _validator = default!;

    [TestInitialize]
    public void Init()
    {
        _validator = new BookAddOptionsValidator();
    }

    [TestMethod]
    public void Validator_ShouldReturnInvalid_WhenBorrowingCountIsLessThanZero()
    {
        var forReadingRoomCount = 31;
        var forBorrowingCount = -4;
        var bookEditionId = 1;
        var options = new BookAddOptions(forReadingRoomCount, forBorrowingCount, bookEditionId);

        var result = _validator.TestValidate(options);

        result.ShouldHaveValidationErrorFor(x => x.ForBorrowingCount);
    }

    [TestMethod]
    public void Validator_ShouldReturnInvalid_WhenReadingRoomCountIsLessThanZero()
    {
        var forReadingRoomCount = -1;
        var forBorrowingCount = 44;
        var bookEditionId = 1;
        var options = new BookAddOptions(forReadingRoomCount, forBorrowingCount, bookEditionId);

        var result = _validator.TestValidate(options);

        result.ShouldHaveValidationErrorFor(x => x.ForReadingRoomCount);
    }

    [TestMethod]
    public void Validator_ShouldReturnValid_WhenBookAddOptionsIsInValidState()
    {
        var forReadingRoomCount = 23;
        var forBorrowingCount = 0;
        var bookEditionId = 1;
        var options = new BookAddOptions(forReadingRoomCount, forBorrowingCount, bookEditionId);

        var result = _validator.TestValidate(options);

        result.ShouldNotHaveAnyValidationErrors();
    }
}