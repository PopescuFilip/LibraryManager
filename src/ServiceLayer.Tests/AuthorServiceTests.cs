using DomainModel;
using FluentValidation;
using NSubstitute;
using ServiceLayer.Authors;
using ServiceLayer.CRUD;
using System.Diagnostics.CodeAnalysis;

namespace ServiceLayer.UnitTests;

[TestClass]
[ExcludeFromCodeCoverage]
public class AuthorServiceTests
{
    private AuthorService _authorService = default!;
    private IEntityService<Author> _entityService = default!;
    private IValidator<Author> _validator = default!;

    [TestInitialize]
    public void Init()
    {
        _entityService = Substitute.For<IEntityService<Author>>();
        _validator = Substitute.For<IValidator<Author>>();
        _authorService = new AuthorService(_entityService, _validator);
    }

    [TestMethod]
    public void Create_ShouldReturnInvalidResult_WhenAuthorIsNotInsertedSuccessfully()
    {
        var name = "namee";
        _entityService.Insert(Arg.Any<Author>(), _validator).Returns(Result.Invalid());

        var result = _authorService.Create(name);

        Assert.IsFalse(result.IsValid);
    }

    [TestMethod]
    public void Create_ShouldReturnValidResult_WhenAuthorIsInsertedSuccessfully()
    {
        var name = "namee";
        _entityService.Insert(Arg.Any<Author>(), _validator)
            .Returns(call => Result.Valid(call.Arg<Author>()));

        var result = _authorService.Create(name);

        Assert.IsTrue(result.IsValid);
        var author = result.Get();
        Assert.AreEqual(name, author.Name);
    }
}