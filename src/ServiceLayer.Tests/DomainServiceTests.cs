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
        entityService.Received(1).Get(
            Arg.Any<Expression<Func<Domain, Domain>>>(),
            Arg.Any<Func<IQueryable<Domain>, Domain?>>(),
            Arg.Any<Expression<Func<Domain, bool>>>(),
            null,
            asNoTracking: true);
    }

    [TestMethod]
    public void Add_ShouldFail_WhenDomainWithNameAlreadyExists()
    {
        var name = "Domain Name";
        var existingDomain = Domain.CreateNew(name);
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
        var existingDomain = Domain.CreateNew(name);
        Expression<Func<Domain, bool>>? receivedFilter = null;

        entityService.Get(
            Arg.Any<Expression<Func<Domain, Domain>>>(),
            Arg.Any<Func<IQueryable<Domain>, Domain?>>(),
            Arg.Is<Expression<Func<Domain, bool>>>(expr => IsFilterForValue(expr, name)),
            null,
            asNoTracking: true)
            .Returns(existingDomain);

        var success = domainService.Add(name);

        Assert.IsFalse(success);
        Assert.IsNotNull(receivedFilter);
        Assert.IsTrue(receivedFilter.Compile()(existingDomain));
    }

    private bool IsFilterForValue(Expression<Func<Domain, bool>> expr, string value)
    {
        var right = (expr.Body as BinaryExpression).Right;
        var left = (expr.Body as BinaryExpression).Left;

        return expr.Body is BinaryExpression binary
            && binary.NodeType == ExpressionType.Equal
            && binary.Right is ConstantExpression constantExpression
            && constantExpression is not null
            && value == (string)constantExpression.Value!;
    }
}