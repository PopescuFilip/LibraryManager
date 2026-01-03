using FluentValidation.TestHelper;
using ServiceLayer.BookDefinitions;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace ServiceLayer.UnitTests;

[TestClass]
[ExcludeFromCodeCoverage]
public class BookDefinitionCreationValidatorTests
{
    private BookDefinitionCreationValidator _validator = default!;

    [TestInitialize]
    public void Init()
    {
        _validator = new BookDefinitionCreationValidator();
    }

    [TestMethod]
    public void Validator_ShouldReturnInvalid_WhenNameIsEmpty()
    {
        var name = string.Empty;
        var authorIds = ImmutableArray.Create(1, 2, 3);
        var domainIds = ImmutableArray.Create(4, 5, 6, 7);
        var options = new BookDefinitionCreateOptions(name, authorIds, domainIds);

        var validationResult = _validator.TestValidate(options);

        validationResult.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [TestMethod]
    public void Validator_ShouldReturnInvalid_WhenAuthorIdsIsEmpty()
    {
        var name = "name";
        var authorIds = ImmutableArray.Create<int>();
        var domainIds = ImmutableArray.Create(4, 5, 6, 7);
        var options = new BookDefinitionCreateOptions(name, authorIds, domainIds);

        var validationResult = _validator.TestValidate(options);

        validationResult.ShouldHaveValidationErrorFor(x => x.AuthorIds.Length);
    }
}
