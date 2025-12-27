using DataMapper;
using DomainModel;
using FluentValidation;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using ServiceLayer.CRUD;
using ServiceLayer.Domains;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace ServiceLayer.UnitTests;

[TestClass]
[ExcludeFromCodeCoverage]
public class DomainServiceTests
{
    private DomainService _domainService = default!;
    private IValidator<Domain> _validator = default!;
    private IEntityService<int, Domain> _entityService = default!;
    private IDomainQueryService _domainQueryService = default!;

    [TestInitialize]
    public void Init()
    {
        _entityService = Substitute.For<IEntityService<int, Domain>>();
        _validator = Substitute.For<IValidator<Domain>>();
        _domainQueryService = Substitute.For<IDomainQueryService>();
        _domainService = new DomainService(_entityService, _domainQueryService, _validator);
    }

    [TestMethod]
    public void Add_ShouldCallRelevantFunctionsForEntityService()
    {
        var name = "Domain Name";
        Domain? actualDomain = null;

        _entityService.Insert(Arg.Any<Domain>(), _validator)
            .Returns(new Result<Domain>(null, true))
            .AndDoes(x => actualDomain = x.Arg<Domain>());

        var success = _domainService.Add(name);

        Assert.IsTrue(success);
        Assert.IsNotNull(actualDomain);
        Assert.AreEqual(name, actualDomain.Name);
        Assert.IsNull(actualDomain.ParentDomainId);
        _domainQueryService.Received(1).GetIdByName(name);
    }

    [TestMethod]
    public void Add_ShouldFail_WhenDomainWithNameAlreadyExists()
    {
        var name = "Domain Name";
        var existingDomainId = 123;

        _domainQueryService.GetIdByName(name).Returns(existingDomainId);

        var success = _domainService.Add(name);

        Assert.IsFalse(success);
        _entityService.DidNotReceiveWithAnyArgs().Insert(default!, default!);
    }

    [TestMethod]
    public void Add_ShouldFail_WhenParentDomainWithNameDoesNotExist()
    {
        var name = "Domain Name";
        var parentDomainName = "Parent domain name";

        _domainQueryService.GetIdByName(name).Returns((int?)null);
        _domainQueryService.GetIdByName(parentDomainName).Returns((int?)null);

        var success = _domainService.Add(name, parentDomainName);

        Assert.IsFalse(success);
        _entityService.DidNotReceiveWithAnyArgs().Insert(default!, default!);
    }

    [TestMethod]
    public void Add_ShouldWork_WhenParentDomainExists()
    {
        var name = "Domain Name";
        var parentDomainName = "Parent domain name";
        var parentDomainId = 143;
        Domain? insertedDomain = null;

        _domainQueryService.GetIdByName(name).Returns((int?)null);
        _domainQueryService.GetIdByName(parentDomainName).Returns(parentDomainId);
        _entityService
            .Insert(Arg.Do<Domain>(d => insertedDomain = d), _validator)
            .Returns(new Result<Domain>(null, true));

        var success = _domainService.Add(name, parentDomainName);

        Assert.IsTrue(success);
        Assert.IsNotNull(insertedDomain);
        Assert.AreEqual(name, insertedDomain.Name);
        Assert.AreEqual(parentDomainId, insertedDomain.ParentDomainId);
        _entityService.ReceivedWithAnyArgs(1).Insert(default!, default!);
    }
}