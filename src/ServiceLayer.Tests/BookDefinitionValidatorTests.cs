using DomainModel;
using DomainModel.Restrictions;
using FluentValidation.TestHelper;
using NSubstitute;
using ServiceLayer.BookDefinitions;
using ServiceLayer.CRUD;
using System.Diagnostics.CodeAnalysis;

namespace ServiceLayer.UnitTests;

[TestClass]
[ExcludeFromCodeCoverage]
public class BookDefinitionValidatorTests
{
    private BookDefinitionValidator _validator = default!;
    private IDomainQueryService _domainQueryService = default!;
    private IBookRestrictionsProvider _restrictionProvider = default!;

    [TestInitialize]
    public void Init()
    {
        _domainQueryService = Substitute.For<IDomainQueryService>();
        _restrictionProvider = Substitute.For<IBookRestrictionsProvider>();
        _validator = new BookDefinitionValidator(_domainQueryService, _restrictionProvider);
    }

    [TestMethod]
    public void Validator_ShouldReturnInvalid_WhenNameIsEmpty()
    {
        var name = string.Empty;
        var authors = new List<Author>()
        {
            new("name1") { Id = 1 },
            new("name2") { Id = 2 },
        };
        var domains = new List<Domain>()
        {
            new("name32") { Id = 4 },
            new("name32") { Id = 4 }
        };
        var bookDefinition = new BookDefinition(name, authors, domains);
        var bookRestriction = new BookRestrictions(domains.Count);
        _restrictionProvider.Get().Returns(bookRestriction);

        var result = _validator.TestValidate(bookDefinition);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }
}