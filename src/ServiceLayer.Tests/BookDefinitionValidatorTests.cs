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
    public void Validator_ShouldReturnInvalid_WhenBookRestrictionsAreNotFound()
    {
        var name = "name";
        var authors = new List<Author>()
        {
            new("name1") { Id = 1 },
            new("name2") { Id = 2 },
        };
        var domains = new List<Domain>()
        {
            new("name32") { Id = 4 },
            new("name33") { Id = 5 }
        };
        var bookDefinition = new BookDefinition(name, authors, domains);
        _restrictionProvider.Get().Returns(Result.Invalid());

        var result = _validator.TestValidate(bookDefinition);

        result.ShouldHaveValidationErrorFor(x => x.Domains.Count);
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
            new("name32") { Id = 5 }
        };
        var bookDefinition = new BookDefinition(name, authors, domains);
        var bookRestriction = new BookRestrictions(domains.Count);
        _restrictionProvider.Get().Returns(Result.Valid(bookRestriction));

        var result = _validator.TestValidate(bookDefinition);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [TestMethod]
    public void Validator_ShouldReturnInvalid_WhenAuthorsIsEmpty()
    {
        var name = "name";
        var authors = new List<Author>();
        var domains = new List<Domain>()
        {
            new("name32") { Id = 4 },
            new("name32") { Id = 5 }
        };
        var bookDefinition = new BookDefinition(name, authors, domains);
        var bookRestriction = new BookRestrictions(domains.Count);
        _restrictionProvider.Get().Returns(Result.Valid(bookRestriction));

        var result = _validator.TestValidate(bookDefinition);

        result.ShouldHaveValidationErrorFor(x => x.Authors.Count);
    }

    [TestMethod]
    public void Validator_ShouldReturnInvalid_WhenDomainsIsEmpty()
    {
        var name = "name";
        var authors = new List<Author>()
        {
            new("name1") { Id = 1 },
            new("name2") { Id = 2 },
        };
        var domains = new List<Domain>();
        var bookDefinition = new BookDefinition(name, authors, domains);
        var bookRestriction = new BookRestrictions(domains.Count);
        _restrictionProvider.Get().Returns(Result.Valid(bookRestriction));

        var result = _validator.TestValidate(bookDefinition);

        result.ShouldHaveValidationErrorFor(x => x.Domains.Count);
    }

    [TestMethod]
    public void Validator_ShouldReturnInvalid_WhenDomainsCountIsHigherThanBookRestrictionAllows()
    {
        var name = "name";
        var authors = new List<Author>()
        {
            new("name1") { Id = 1 },
            new("name2") { Id = 2 },
        };
        var domains = new List<Domain>()
        {
            new("name32") { Id = 4 },
            new("name33") { Id = 5 },
            new("name45") { Id = 6 }
        };
        var bookDefinition = new BookDefinition(name, authors, domains);
        var bookRestriction = new BookRestrictions(domains.Count - 1);
        _restrictionProvider.Get().Returns(Result.Valid(bookRestriction));

        var result = _validator.TestValidate(bookDefinition);

        result.ShouldHaveValidationErrorFor(x => x.Domains.Count);
    }

    [TestMethod]
    public void Validator_ShouldReturnInvalid_WhenDomainsExplicitlyContainsImplicitDomains()
    {
        var name = "name";
        var authors = new List<Author>()
        {
            new("name1") { Id = 1 },
            new("name2") { Id = 2 },
        };
        var domains = new List<Domain>()
        {
            new("name32") { Id = 4 },
            new("name33") { Id = 5 },
            new("name45") { Id = 6 }
        };
        var bookDefinition = new BookDefinition(name, authors, domains);
        var bookRestriction = new BookRestrictions(domains.Count - 1);
        _restrictionProvider.Get().Returns(Result.Valid(bookRestriction));
        var implicitDomains = domains.Select(x => x.Name).Take(1);
        var domainIds = domains.Select(x => x.Id);
        _domainQueryService
            .GetImplicitDomainNames(Arg.Is<IEnumerable<int>>(x => x.SequenceEqual(domainIds)))
            .Returns(implicitDomains);

        var result = _validator.TestValidate(bookDefinition);

        result.ShouldHaveValidationErrorFor(x => x.Domains);
    }

    [TestMethod]
    public void Validator_ShouldReturnValid_WhenBookDefinitionIsValid()
    {
        var name = "name";
        var authors = new List<Author>()
        {
            new("name1") { Id = 1 },
            new("name2") { Id = 2 },
        };
        var domains = new List<Domain>()
        {
            new("name32") { Id = 4 },
            new("name33") { Id = 5 }
        };
        var bookDefinition = new BookDefinition(name, authors, domains);
        var bookRestriction = new BookRestrictions(domains.Count);
        _restrictionProvider.Get().Returns(Result.Valid(bookRestriction));

        var result = _validator.TestValidate(bookDefinition);

        result.ShouldNotHaveAnyValidationErrors();
    }
}