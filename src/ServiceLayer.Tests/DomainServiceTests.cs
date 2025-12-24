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
    private DomainService domainService = default!;
    private IValidator<Domain> validator = default!;
    private IEntityService<IDomainRepository, int, Domain> entityService = default!;

    [TestInitialize]
    public void Init()
    {
        entityService = Substitute.For<IEntityService<IDomainRepository, int, Domain>>();
        validator = Substitute.For<IValidator<Domain>>();
        domainService = new DomainService(entityService, validator);
    }

    [TestMethod]
    public void Add_ShouldCallRelevantFunctionsForEntityService()
    {
        var name = "Domain Name";
        Domain? actualDomain = null;

        entityService.Insert(Arg.Any<Domain>(), validator)
            .Returns(true)
            .AndDoes(x => actualDomain = x.Arg<Domain>());

        var success = domainService.Add(name);

        Assert.IsTrue(success);
        Assert.IsNotNull(actualDomain);
        Assert.AreEqual(name, actualDomain.Name);
        entityService
            .ReceivedWithAnyArgs(1)
            .Get<Domain, Domain?>(default!, default!, default, default, default);
    }

    [TestMethod]
    public void Add_ShouldFail_WhenDomainWithNameAlreadyExists()
    {
        var name = "Domain Name";
        var existingDomain = new Domain(name);
        Expression<Func<Domain, bool>>? receivedFilter = null;

        entityService.Get(
            Arg.Any<Expression<Func<Domain, Domain>>>(),
            Arg.Any<Func<IQueryable<Domain>, Domain?>>(),
            Arg.Do<Expression<Func<Domain, bool>>>(filter => receivedFilter = filter),
            null,
            asNoTracking: true)
            .Returns(existingDomain);

        var success = domainService.Add(name);

        Assert.IsFalse(success);
        Assert.IsNotNull(receivedFilter);
        Assert.IsTrue(receivedFilter.Compile()(existingDomain));
    }

    [TestMethod]
    public void Add_ShouldFail_WhenParentDomainWithNameDoesNotExist()
    {
        var name = "Domain Name";
        var parentDomain = "Parent domain name";

        entityService
            .Get<Domain, Domain?>(default!, default!, default, default, default)
            .ReturnsForAnyArgs((Domain?)null);

        var success = domainService.Add(name, parentDomain);

        Assert.IsFalse(success);
        entityService
            .ReceivedWithAnyArgs(2)
            .Get<Domain, Domain?>(default!, default!, default, default, default);
    }

    [TestMethod]
    public void Add_ShouldWork_WhenParentDomainExists()
    {
        var name = "Domain Name";
        var parentDomainName = "Parent domain name";
        var parentDomain = new Domain(parentDomainName, null) { Id = 123 };
        Domain? insertedDomain = null;

        entityService
            .Get<Domain, Domain?>(default!, default!, default, default, default)
            .ReturnsForAnyArgs(null, parentDomain);
        entityService
            .Insert(Arg.Do<Domain>(d => insertedDomain = d), validator)
            .Returns(true);

        var success = domainService.Add(name, parentDomainName);

        Assert.IsTrue(success);
        entityService
            .ReceivedWithAnyArgs(2)
            .Get<Domain, Domain?>(default!, default!, default, default, default);
        entityService.ReceivedWithAnyArgs(1).Insert(default!, default!);
        Assert.IsNotNull(insertedDomain);
        Assert.AreEqual(name, insertedDomain.Name);
        Assert.AreEqual(parentDomain.Id, insertedDomain.ParentDomainId);
    }
}