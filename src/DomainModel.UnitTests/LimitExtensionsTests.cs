using DomainModel.Restrictions;
using System.Diagnostics.CodeAnalysis;

namespace DomainModel.UnitTests;

[TestClass]
[ExcludeFromCodeCoverage]
public class LimitExtensionsTests
{
    [TestMethod]
    public void DoubleItem_ShouldOnlyDoubleItemCount()
    {
        var itemCount = 32;
        var initialLimit = Limit.PerRequest(itemCount);
        var expectedLimit = Limit.PerRequest(itemCount * 2);

        var actualLimit = initialLimit.DoubleItem();

        Assert.AreEqual(expectedLimit, actualLimit);
    }

    [TestMethod]
    public void HalfTime_ShouldOnlyHalfTimeUnitCount()
    {
        var itemCount = 32;
        var timeUnitCount = 2;
        var initialLimit = Limit.PerPeriodInMonths(itemCount, timeUnitCount);
        var expectedLimit = Limit.PerPeriodInMonths(itemCount, timeUnitCount / 2);

        var actualLimit = initialLimit.HalfTime();

        Assert.AreEqual(expectedLimit, actualLimit);
    }

    [TestMethod]
    public void HalfTime_ShouldNotChangeTimeUnit_WhenItIsOne()
    {
        var itemCount = 32;
        var timeUnitCount = 1;
        var initialLimit = Limit.PerPeriodInMonths(itemCount, timeUnitCount);

        var actualLimit = initialLimit.HalfTime();

        Assert.AreEqual(initialLimit, actualLimit);
    }

    [TestMethod]
    public void GetStartTimeToCheck_ShouldReturnCorrectStartTime_WhenLimitIsPerPeriodInMonths()
    {
        var itemCount = 32;
        var timeUnitCount = 40;
        var limit = Limit.PerPeriodInMonths(itemCount, timeUnitCount);
        var expectedDateTime = DateTime.Now.AddMonths(-timeUnitCount);

        var actualDateTime = limit.GetStartTimeToCheck();

        var difference = actualDateTime - expectedDateTime;
        Assert.IsTrue(difference.Minutes < 1);
    }

    [TestMethod]
    public void GetStartTimeToCheck_ShouldReturnCorrectStartTime_WhenLimitIsPerPeriodInDays()
    {
        var itemCount = 32;
        var timeUnitCount = 40;
        var limit = Limit.PerPeriodInDays(itemCount, timeUnitCount);
        var expectedDateTime = DateTime.Now.AddDays(-timeUnitCount);

        var actualDateTime = limit.GetStartTimeToCheck();

        var difference = actualDateTime - expectedDateTime;
        Assert.IsTrue(difference.Minutes < 1);
    }

    [TestMethod]
    public void GetStartTimeToCheck_ShouldReturnCorrectStartTime_WhenLimitIsPerDay()
    {
        var itemCount = 32;
        var limit = Limit.PerDay(itemCount);

        var actualDateTime = limit.GetStartTimeToCheck();

        Assert.AreEqual(DateTime.Today, actualDateTime);
    }
}