using DomainModel;
using FluentValidation;
using FluentValidation.Results;
using NSubstitute;
using ServiceLayer.BookDefinitions;
using ServiceLayer.CRUD;
using ServiceLayer.UnitTests.TestHelpers;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace ServiceLayer.UnitTests;

[TestClass]
[ExcludeFromCodeCoverage]
public class BookDefinitionServiceTests
{
    private BookDefinitionService _bookDefinitionService = default!;
    private IEntityService<BookDefinition> _bookEntityService = default!;
    private IEntityService<Author> _authorEntityService = default!;
    private IEntityService<Domain> _domainEntityService = default!;
    private IValidator<BookDefinitionCreateOptions> _optionsValidator = default!;
    private IValidator<BookDefinition> _bookDefinitionValidator = default!;

    [TestInitialize]
    public void Init()
    {
        _bookEntityService = Substitute.For<IEntityService<BookDefinition>>();
        _authorEntityService = Substitute.For<IEntityService<Author>>();
        _domainEntityService = Substitute.For<IEntityService<Domain>>();
        _optionsValidator = Substitute.For<IValidator<BookDefinitionCreateOptions>>();
        _bookDefinitionValidator = Substitute.For<IValidator<BookDefinition>>();
        _bookDefinitionService = new BookDefinitionService(
            _bookEntityService,
            _authorEntityService,
            _domainEntityService,
            _optionsValidator,
            _bookDefinitionValidator);
    }

    [TestMethod]
    public void Create_ShouldReturnInvalidResult_WhenOptionValidationFails()
    {
        var name = "name";
        var authorIds = ImmutableArray.Create(1, 2);
        var domainIds = ImmutableArray.Create(3, 6);
        var authors = Generator.GenerateAuthorsFrom(authorIds);
        var domains = Generator.GenerateDomainsFrom(domainIds);
        var options = new BookDefinitionCreateOptions(name, authorIds, domainIds);
        _optionsValidator.Validate(options).Returns(Validation.InvalidResult);
        _bookEntityService.Insert(Arg.Any<BookDefinition>(), _bookDefinitionValidator)
            .Returns(call => Result.Valid(call.Arg<BookDefinition>()));
        _domainEntityService.GetAllById(domainIds).Returns(domains);
        _authorEntityService.GetAllById(authorIds).Returns(authors);

        var result = _bookDefinitionService.Create(options);

        Assert.IsFalse(result.IsValid);
    }

    [TestMethod]
    public void Create_ShouldReturnInvalidResult_WhenInsertionFails()
    {
        var name = "name";
        var authorIds = ImmutableArray.Create(1, 2);
        var domainIds = ImmutableArray.Create(3, 6);
        var authors = Generator.GenerateAuthorsFrom(authorIds);
        var domains = Generator.GenerateDomainsFrom(domainIds);
        var options = new BookDefinitionCreateOptions(name, authorIds, domainIds);
        var invalidResult = Result.Invalid<BookDefinition>();
        _optionsValidator.Validate(options).Returns(Validation.ValidResult);
        _bookEntityService.Insert(Arg.Any<BookDefinition>(), _bookDefinitionValidator)
            .Returns(invalidResult);
        _domainEntityService.GetAllById(domainIds).Returns(domains);
        _authorEntityService.GetAllById(authorIds).Returns(authors);

        var result = _bookDefinitionService.Create(options);

        Assert.IsFalse(result.IsValid);
        Assert.AreSame(invalidResult, result);
    }

    [TestMethod]
    public void Create_ShouldInsertBookDefinitionAndReturnValidResult_WhenAllValidationsPass()
    {
        var name = "name";
        var authorIds = ImmutableArray.Create(1, 2);
        var domainIds = ImmutableArray.Create(3, 6);
        var authors = Generator.GenerateAuthorsFrom(authorIds);
        var domains = Generator.GenerateDomainsFrom(domainIds);
        var options = new BookDefinitionCreateOptions(name, authorIds, domainIds);
        _optionsValidator.Validate(options).Returns(Validation.ValidResult);
        _bookEntityService.Insert(Arg.Any<BookDefinition>(), _bookDefinitionValidator)
            .Returns(call => Result.Valid(call.Arg<BookDefinition>()));
        _domainEntityService.GetAllById(domainIds).Returns(domains);
        _authorEntityService.GetAllById(authorIds).Returns(authors);

        var result = _bookDefinitionService.Create(options);

        Assert.IsTrue(result.IsValid);
        var bookDefinition = result.Get();
        Assert.AreEqual(name, bookDefinition.Name);
        Assert.IsTrue(authors.SequenceEqual(bookDefinition.Authors));
        Assert.IsTrue(domains.SequenceEqual(bookDefinition.Domains));
    }
}