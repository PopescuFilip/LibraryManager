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

    [TestMethod]
    public void GetImplicitDomainNames_ShouldReturnCorrectValues_WhenReceivingOneId()
    {
        var parent2 = new Domain("nam3") { Id = 26 };
        var parent1 = new Domain("nam1", parent2.Id)
        {
            Id = 24,
            ParentDomain = parent2
        };
        var interestingDomain = new Domain("name1", parent1.Id)
        {
            Id = 1,
            ParentDomain = parent1,
        };
        var otherDomain = new Domain("otherDomain", parent1.Id)
        {
            Id = 54,
            ParentDomain = parent1
        };
        var ids = new List<int>() { interestingDomain.Id };
        var domains = new List<Domain>() { interestingDomain, parent1, parent2, otherDomain };
        var expectedNames = new List<Domain>() { parent1, parent2 }
            .Select(x => x.Name);
        _repository.SetSourceValues(domains);

        var actualNames = _domainQueryService.GetImplicitDomainNames(ids);

        Assert.IsTrue(actualNames.SequenceEqual(expectedNames));
    }

    [TestMethod]
    public void GetImplicitDomainNames_ShouldReturnCorrectValues_WhenReceivingMultipleIds()
    {
        var parent2 = new Domain("nam3") { Id = 26 };
        var parent1 = new Domain("nam1", parent2.Id)
        {
            Id = 24,
            ParentDomain = parent2
        };
        var domain = new Domain("name1", parent1.Id)
        {
            Id = 1,
            ParentDomain = parent1,
        };
        var otherDomain = new Domain("otherDomain", parent1.Id)
        {
            Id = 54,
            ParentDomain = parent1
        };
        var separateParent = new Domain("separateParent", parent2.Id)
        {
            Id = 21,
            ParentDomain = parent2,
        };
        var separateDomain = new Domain("separateDomain", separateParent.Id)
        {
            Id = 22,
            ParentDomain = separateParent,
        };
        var ids = new List<int>() { domain.Id, separateDomain.Id };
        var domains = new List<Domain>() { domain, parent1, parent2, otherDomain, separateParent, separateDomain };
        var expectedNames = new List<Domain>() { parent1, parent2, separateParent, parent2 }
            .Select(x => x.Name);
        _repository.SetSourceValues(domains);

        var actualNames = _domainQueryService.GetImplicitDomainNames(ids);

        Assert.IsTrue(actualNames.SequenceEqual(expectedNames));
    }

    [TestMethod]
    public void GetImplicitDomainNames_ShouldReturnNothing_WhenIdBelongsToDomainWithNoParent()
    {
        var parent2 = new Domain("nam3") { Id = 26 };
        var parent1 = new Domain("nam1", parent2.Id)
        {
            Id = 24,
            ParentDomain = parent2
        };
        var domain = new Domain("name1", parent1.Id)
        {
            Id = 1,
            ParentDomain = parent1,
        };
        var otherDomain = new Domain("otherDomain", parent1.Id)
        {
            Id = 54,
            ParentDomain = parent1
        };
        var ids = new List<int>() { parent2.Id };
        var domains = new List<Domain>() { domain, parent1, parent2, otherDomain };
        _repository.SetSourceValues(domains);

        var actualNames = _domainQueryService.GetImplicitDomainNames(ids);

        Assert.IsFalse(actualNames.Any());
    }

    [TestMethod]
    public void GetImplicitDomainNames_ShouldReturnNothing_WhenIdDoesNotExist()
    {
        var nonexistentId = 431;
        var parent2 = new Domain("nam3") { Id = 26 };
        var parent1 = new Domain("nam1", parent2.Id)
        {
            Id = 24,
            ParentDomain = parent2
        };
        var domain = new Domain("name1", parent1.Id)
        {
            Id = 1,
            ParentDomain = parent1,
        };
        var otherDomain = new Domain("otherDomain", parent1.Id)
        {
            Id = 54,
            ParentDomain = parent1
        };
        var ids = new List<int>() { nonexistentId };
        var domains = new List<Domain>() { domain, parent1, parent2, otherDomain };
        _repository.SetSourceValues(domains);

        var actualNames = _domainQueryService.GetImplicitDomainNames(ids);

        Assert.IsFalse(actualNames.Any());
    }
}