using DomainModel;
using ServiceLayer.CRUD;
using ServiceLayer.Exceptions;
using ServiceLayer.UnitTests.TestHelpers;
using System.Diagnostics.CodeAnalysis;

namespace ServiceLayer.UnitTests;

[TestClass]
[ExcludeFromCodeCoverage]
public class DomainQueryServiceTests
{
    private DomainQueryService _domainQueryService = default!;
    private FakeRepository<Domain> _repository = default!;

    [TestInitialize]
    public void Init()
    {
        _repository = new FakeRepository<Domain>();
        _domainQueryService = new DomainQueryService(_repository);
    }

    [TestMethod]
    public void GetIdByName_ShouldReturnIdForNameWhenItExists()
    {
        var interestingName = "interestingName";
        var interestingId = 123;
        var idName = new List<(int Id, string Name)>()
        {
            (1, "name"),
            (2, "name324"),
            (4356, "nameNAME"),
            (interestingId, interestingName),
            (43456, "naeNAE")
        };

        _repository.SetSourceValues(DomainGenerator.From(idName));

        var actualId = _domainQueryService.GetIdByName(interestingName);

        Assert.AreEqual(interestingId, actualId);
    }

    [TestMethod]
    public void GetIdByName_ShouldReturnNullWhenNameDoesNotExist()
    {
        var interestingName = "interestingName";
        var idName = new List<(int Id, string Name)>()
        {
            (1, "name"),
            (2, "name324"),
            (4356, "nameNAME"),
            (43456, "naeNAE")
        };

        _repository.SetSourceValues(DomainGenerator.From(idName));

        var actualId = _domainQueryService.GetIdByName(interestingName);

        Assert.IsNull(actualId);
    }

    [TestMethod]
    public void GetIdByName_ShouldThrowWhenMoreThanOneDomainWithNameIsFound()
    {
        var interestingName = "interestingName";
        var idName = new List<(int Id, string Name)>()
        {
            (1, "name"),
            (2, "name324"),
            (722, interestingName),
            (999, interestingName),
            (46, "nameNAME"),
            (436, "naeNAE")
        };

        _repository.SetSourceValues(DomainGenerator.From(idName));

        void CallGetByName() => _domainQueryService.GetIdByName(interestingName);

        Assert.ThrowsException<DuplicateDomainNameException>(CallGetByName);
    }
}