using DomainModel.Restrictions;
using NSubstitute;
using ServiceLayer.CRUD;
using ServiceLayer.Restriction;
using System.Diagnostics.CodeAnalysis;

namespace ServiceLayer.UnitTests;

[TestClass]
[ExcludeFromCodeCoverage]
public class RestrictionsServiceTests
{
    private RestrictionsService _restrictionsService = default!;
    private IClientRestrictionsProvider _restrictionsProvider = default!;
    private IAccountQueryService _accountQueryService = default!;

    [TestInitialize]
    public void Init()
    {
        _restrictionsProvider = Substitute.For<IClientRestrictionsProvider>();
        _accountQueryService = Substitute.For<IAccountQueryService>();

        _restrictionsService = new RestrictionsService(
            _restrictionsProvider,
            _accountQueryService);
    }

    [TestMethod]
    public void GetRestrictionsForAccount_ShouldReturnInvalid_WhenEmployeeForAccountDoesNotExistAndRestrictionsAreNotFound()
    {
        var accountId = 321;
        var normalRestrictions = new ClientRestrictions(
            Limit.PerPeriodInDays(1, 3),
            default!,
            default!,
            default!,
            default!,
            default!);
        var privilegedRestrictions = normalRestrictions with
        {
            BorrowedBooksLimit = Limit.PerPeriodInMonths(4, 13),
        };
        _accountQueryService.ClientForAccountExists(accountId).Returns(true);
        _accountQueryService.EmployeeForAccountExists(accountId).Returns(true);
        _restrictionsProvider.GetClientRestrictions()
            .Returns(Result.Invalid());
        _restrictionsProvider.GetPrivilegedClientRestrictions()
            .Returns(Result.Invalid());

        var result = _restrictionsService.GetRestrictionsForAccount(accountId);

        Assert.IsFalse(result.IsValid);
    }

    [TestMethod]
    public void GetRestrictionsForAccount_ShouldReturnInvalid_WhenEmployeeForAccountExistsAndRestrictionsAreNotFound()
    {
        var accountId = 321;
        var normalRestrictions = new ClientRestrictions(
            Limit.PerPeriodInDays(1, 3),
            default!,
            default!,
            default!,
            default!,
            default!);
        _accountQueryService.ClientForAccountExists(accountId).Returns(true);
        _accountQueryService.EmployeeForAccountExists(accountId).Returns(true);
        _restrictionsProvider.GetClientRestrictions()
            .Returns(Result.Valid(normalRestrictions));
        _restrictionsProvider.GetPrivilegedClientRestrictions()
            .Returns(Result.Invalid());

        var result = _restrictionsService.GetRestrictionsForAccount(accountId);

        Assert.IsFalse(result.IsValid);
    }

    [TestMethod]
    public void GetRestrictionsForAccount_ShouldReturnInvalid_WhenClientForAccountDoestNotExist()
    {
        var accountId = 321;
        var normalRestrictions = new ClientRestrictions(
            Limit.PerPeriodInDays(1, 3),
            default!,
            default!,
            default!,
            default!,
            default!);
        var privilegedRestrictions = normalRestrictions with
        {
            BorrowedBooksLimit = Limit.PerPeriodInMonths(4, 13),
        };
        _accountQueryService.ClientForAccountExists(accountId).Returns(false);
        _accountQueryService.EmployeeForAccountExists(accountId).Returns(false);
        _restrictionsProvider.GetClientRestrictions()
            .Returns(Result.Valid(normalRestrictions));
        _restrictionsProvider.GetPrivilegedClientRestrictions()
            .Returns(Result.Valid(privilegedRestrictions));

        var result = _restrictionsService.GetRestrictionsForAccount(accountId);

        Assert.IsFalse(result.IsValid);
    }

    [TestMethod]
    public void GetRestrictionsForAccount_ShouldReturnNormalRestrictions_WhenEmployeeForAccountDoesNotExist()
    {
        var accountId = 321;
        var normalRestrictions = new ClientRestrictions(
            Limit.PerPeriodInDays(1, 3),
            default!,
            default!,
            default!,
            default!,
            default!);
        var privilegedRestrictions = normalRestrictions with
        {
            BorrowedBooksLimit = Limit.PerPeriodInMonths(4, 13),
        };
        _accountQueryService.ClientForAccountExists(accountId).Returns(true);
        _accountQueryService.EmployeeForAccountExists(accountId).Returns(false);
        _restrictionsProvider.GetClientRestrictions()
            .Returns(Result.Valid(normalRestrictions));
        _restrictionsProvider.GetPrivilegedClientRestrictions()
            .Returns(Result.Valid(privilegedRestrictions));

        var result = _restrictionsService.GetRestrictionsForAccount(accountId);

        Assert.IsTrue(result.IsValid);
        var restrictions = result.Get();
        Assert.AreEqual(normalRestrictions, restrictions);
    }

    [TestMethod]
    public void GetRestrictionsForAccount_ShouldReturnPrivilegedRestrictions_WhenEmployeeForAccountExists()
    {
        var accountId = 321;
        var normalRestrictions = new ClientRestrictions(
            Limit.PerPeriodInDays(1, 3),
            default!,
            default!,
            default!,
            default!,
            default!);
        var privilegedRestrictions = normalRestrictions with
        {
            BorrowedBooksLimit = Limit.PerPeriodInMonths(4, 13),
        };
        _accountQueryService.ClientForAccountExists(accountId).Returns(true);
        _accountQueryService.EmployeeForAccountExists(accountId).Returns(true);
        _restrictionsProvider.GetClientRestrictions()
            .Returns(Result.Valid(normalRestrictions));
        _restrictionsProvider.GetPrivilegedClientRestrictions()
            .Returns(Result.Valid(privilegedRestrictions));

        var result = _restrictionsService.GetRestrictionsForAccount(accountId);

        Assert.IsTrue(result.IsValid);
        var restrictions = result.Get();
        Assert.AreEqual(privilegedRestrictions, restrictions);
    }
}