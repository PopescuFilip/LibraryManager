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
        var idValidator = new IdCollectionValidator();
        _validator = new BookDefinitionCreationValidator(idValidator);
    }

    [TestMethod]
    public void Validator_ShouldReturnInvalid_WhenNameIsEmpty()
    {
        var name = string.Empty;
        var authorIds = ImmutableArray.Create(1, 2, 3).ToIdCollection();
        var domainIds = ImmutableArray.Create(4, 5, 6, 7).ToIdCollection();
        var options = new BookDefinitionCreateOptions(name, authorIds, domainIds);

        var validationResult = _validator.TestValidate(options);

        validationResult.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [TestMethod]
    public void Validator_ShouldReturnInvalid_WhenAuthorIdsIsEmpty()
    {
        var name = "name";
        var authorIds = ImmutableArray.Create<int>().ToIdCollection();
        var domainIds = ImmutableArray.Create(4, 5, 6, 7).ToIdCollection();
        var options = new BookDefinitionCreateOptions(name, authorIds, domainIds);

        var validationResult = _validator.TestValidate(options);

        validationResult.ShouldHaveValidationErrorFor(x => x.AuthorIds.Count);
    }

    [TestMethod]
    public void Validator_ShouldReturnInvalid_WhenDomainIdsIsEmpty()
    {
        var name = "name";
        var authorIds = ImmutableArray.Create(1, 2, 3).ToIdCollection();
        var domainIds = ImmutableArray.Create<int>().ToIdCollection();
        var options = new BookDefinitionCreateOptions(name, authorIds, domainIds);

        var validationResult = _validator.TestValidate(options);

        validationResult.ShouldHaveValidationErrorFor(x => x.DomainIds.Count);
    }

    [TestMethod]
    public void Validator_ShouldReturnInvalid_WhenAuthorIdsContainsDuplicates()
    {
        var name = "name";
        var authorIds = ImmutableArray.Create(1, 2, 3, 3).ToIdCollection();
        var domainIds = ImmutableArray.Create(4, 5, 6, 7).ToIdCollection();
        var options = new BookDefinitionCreateOptions(name, authorIds, domainIds);

        var validationResult = _validator.TestValidate(options);

        validationResult.ShouldHaveValidationErrorFor(x => x.AuthorIds);
    }

    [TestMethod]
    public void Validator_ShouldReturnInvalid_WhenDomainIdsContainsDuplicates()
    {
        var name = "name";
        var authorIds = ImmutableArray.Create(1, 2, 3).ToIdCollection();
        var domainIds = ImmutableArray.Create(4, 5, 6, 7, 4).ToIdCollection();
        var options = new BookDefinitionCreateOptions(name, authorIds, domainIds);

        var validationResult = _validator.TestValidate(options);

        validationResult.ShouldHaveValidationErrorFor(x => x.DomainIds);
    }

    [TestMethod]
    public void Validator_ShouldReturnValid_WhenOptionsAreInCorrectState()
    {
        var name = "name";
        var authorIds = ImmutableArray.Create(1, 2, 3).ToIdCollection();
        var domainIds = ImmutableArray.Create(4, 5, 6, 7).ToIdCollection();
        var options = new BookDefinitionCreateOptions(name, authorIds, domainIds);

        var validationResult = _validator.TestValidate(options);

        validationResult.ShouldNotHaveAnyValidationErrors();
    }
}