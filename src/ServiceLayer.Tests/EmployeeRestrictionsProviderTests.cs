using DataMapper;
using DomainModel.Restrictions;
using NSubstitute;
using ServiceLayer.CRUD;
using System.Diagnostics.CodeAnalysis;

namespace ServiceLayer.UnitTests;

[TestClass]
[ExcludeFromCodeCoverage]
public class EmployeeRestrictionsProviderTests
{
    private EmployeeRestrictionsProvider _employeeRestrictionsProvider = default!;
    private IRestrictionsProvider _restrictionsProvider = default!;

    [TestInitialize]
    public void Init()
    {
        _restrictionsProvider = Substitute.For<IRestrictionsProvider>();
        _employeeRestrictionsProvider = new EmployeeRestrictionsProvider(_restrictionsProvider);
    }

    [TestMethod]
    public void Get_ShouldReturnValid_WhenNoRestrictionsAreFound()
    {
        _restrictionsProvider.GetRestrictions().Returns((RawRestrictions?)null);

        var result = _employeeRestrictionsProvider.Get();

        Assert.IsFalse(result.IsValid);
    }

    [TestMethod]
    public void Get_ShouldReturnCorrectRestrictions_WhenRestrictionsProviderReturnsRestrictions()
    {
        var restrictions = new RawRestrictions()
        {
            MaxBorrowedBooksGivenPerDay = 34
        };
        _restrictionsProvider.GetRestrictions().Returns(restrictions);
        var expectedLimit = new PerDayLimit(restrictions.MaxBorrowedBooksGivenPerDay);

        var result = _employeeRestrictionsProvider.Get();

        Assert.IsTrue(result.IsValid);
        var employeeRestrictions = result.Get();
        Assert.AreEqual(expectedLimit, employeeRestrictions.BorrowedBooksGivenLimit);
    }

    [TestMethod]
    public void Get_ShouldReturnValid_WhenRestrictionsProviderReturnsRestrictions()
    {
        var restrictions = new RawRestrictions()
        {
            MaxBorrowedBooksGivenPerDay = 34
        };
        _restrictionsProvider.GetRestrictions().Returns(restrictions);

        var result = _employeeRestrictionsProvider.Get();

        Assert.IsTrue(result.IsValid);
    }
}