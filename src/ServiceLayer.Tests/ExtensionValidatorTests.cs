using DomainModel;
using FluentValidation.TestHelper;
using ServiceLayer.Borrowing;
using System.Diagnostics.CodeAnalysis;

namespace ServiceLayer.UnitTests;

[TestClass]
[ExcludeFromCodeCoverage]
public class ExtensionValidatorTests
{
    private ExtensionValidator _validator = default!;

    [TestInitialize]
    public void Init()
    {
        _validator = new ExtensionValidator();
    }

    [TestMethod]
    public void Validator_ShouldReturnInvalid_WhenCreatedTimeIsInTheFuture()
    {
        var extension = new Extension(1, 0)
        {
            CreatedDateTime = DateTime.Now.AddDays(1),
        };

        var result = _validator.TestValidate(extension);

        result.ShouldHaveValidationErrorFor(x => x.CreatedDateTime);
    }

    [TestMethod]
    public void Validator_ShouldReturnInvalid_WhenDaysCountIsNegative()
    {
        var extension = new Extension(1, -3);

        var result = _validator.TestValidate(extension);

        result.ShouldHaveValidationErrorFor(x => x.DayCount);
    }

    [TestMethod]
    public void Validator_ShouldReturnInvalid_WhenDaysCountIsZero()
    {
        var extension = new Extension(1, 0);

        var result = _validator.TestValidate(extension);

        result.ShouldHaveValidationErrorFor(x => x.DayCount);
    }

    [TestMethod]
    public void Validator_ShouldReturnValid_WhenExtensionIsInValidState()
    {
        var extension = new Extension(1, 3);

        var result = _validator.TestValidate(extension);

        result.ShouldNotHaveAnyValidationErrors();
    }
}