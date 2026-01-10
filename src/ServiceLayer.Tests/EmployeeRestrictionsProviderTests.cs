using DataMapper;
using DomainModel.Restrictions;
using NSubstitute;
using ServiceLayer.CRUD;

namespace ServiceLayer.UnitTests;

[TestClass]
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
    public void Get_ShouldReturnCorrectRestrictions()
    {
        var restrictions = new RawRestrictions()
        {
            MaxBorrowedBooksGivenPerDay = 34
        };
        _restrictionsProvider.GetRestrictions().Returns(restrictions);
        var expectedLimit = new PerDayLimit(restrictions.MaxBorrowedBooksGivenPerDay);

        var employeeRestrictions = _employeeRestrictionsProvider.Get();

        Assert.AreEqual(expectedLimit, employeeRestrictions.BorrowedBooksGivenLimit);
    }
}