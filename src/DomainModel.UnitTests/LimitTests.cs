using DomainModel.Restrictions;
using System.Diagnostics.CodeAnalysis;

namespace DomainModel.UnitTests;

[TestClass]
[ExcludeFromCodeCoverage]
public class LimitTests
{
    [TestMethod]
    public void PerDay_ShouldConstructCorrectLimit()
    {
        var itemCount = 10;
        var expectedLimit = new PerDayLimit(itemCount);

        var limit = Limit.PerDay(itemCount);

        Assert.AreEqual(expectedLimit, limit);
    }

    [TestMethod]
    public void PerRequest_ShouldConstructCorrectLimit()
    {
        var itemCount = 10;
        var expectedLimit = new PerRequestLimit(itemCount);

        var limit = Limit.PerRequest(itemCount);

        Assert.AreEqual(expectedLimit, limit);
    }

    [TestMethod]
    public void PerPeriodInDays_ShouldConstructCorrectLimit()
    {
        var itemCount = 10;
        var timeUnitCount = 10;
        var timeUnit = TimeUnit.Day;
        var expectedLimit = new PeriodLimit(itemCount, timeUnitCount, timeUnit);

        var limit = Limit.PerPeriodInDays(itemCount, timeUnitCount);

        Assert.AreEqual(expectedLimit, limit);
    }

    [TestMethod]
    public void PerPeriodInMonths_ShouldConstructCorrectLimit()
    {
        var itemCount = 10;
        var timeUnitCount = 10;
        var timeUnit = TimeUnit.Month;
        var expectedLimit = new PeriodLimit(itemCount, timeUnitCount, timeUnit);

        var limit = Limit.PerPeriodInMonths(itemCount, timeUnitCount);

        Assert.AreEqual(expectedLimit, limit);
    }
}