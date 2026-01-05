using DomainModel;
using FluentValidation.TestHelper;
using ServiceLayer.Authors;
using System.Diagnostics.CodeAnalysis;

namespace ServiceLayer.UnitTests;

[TestClass]
[ExcludeFromCodeCoverage]
public class AuthorValidatorTests
{
    private AuthorValidator _authorValidator = default!;

    [TestInitialize]
    public void Init()
    {
        _authorValidator = new AuthorValidator();
    }

    [TestMethod]
    public void Validator_ShouldReturnInvalid_WhenNameIsEmpty()
    {
        var author = new Author(string.Empty);

        var result = _authorValidator.TestValidate(author);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [TestMethod]
    public void Validator_ShouldReturnValid_WhenAuthorIsInValidState()
    {
        var author = new Author("name");

        var result = _authorValidator.TestValidate(author);

        result.ShouldNotHaveAnyValidationErrors();
    }
}