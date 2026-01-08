using DataMapper;
using DomainModel.Restrictions;
using NSubstitute;
using ServiceLayer.CRUD;
using System.Diagnostics.CodeAnalysis;

namespace ServiceLayer.UnitTests;

[TestClass]
[ExcludeFromCodeCoverage]
public class ClientRestrictionsProviderTests
{
    private ClientRestrictionsProvider _clientRestrictionsProvider = default!;
    private IRestrictionsProvider _restrictionsProvider = default!;

    [TestInitialize]
    public void Init()
    {
        _restrictionsProvider = Substitute.For<IRestrictionsProvider>();
        _clientRestrictionsProvider = new ClientRestrictionsProvider(_restrictionsProvider);
    }

    [TestMethod]
    public void GetClientRestrictions_ShouldMapMaxBooksBorrowedPerDayPropertiesCorrectly()
    {
        var restrictions = new RawRestrictions()
        {
            MaxBorrowedBooksPerDay = 15
        };
        _restrictionsProvider.GetRestrictions().Returns(restrictions);

        var clientRestrictions = _clientRestrictionsProvider.GetClientRestrictions();

        var perDayLimit = clientRestrictions.BorrowedBooksPerDayLimit.ItemCount;
        Assert.AreEqual(restrictions.MaxBorrowedBooksPerDay, perDayLimit);
    }

    [TestMethod]
    public void GetClientRestrictions_ShouldMapSameBookLimitPropertiesCorrectly()
    {
        var restrictions = new RawRestrictions()
        {
            SameBookLimitDayCount = 10
        };
        _restrictionsProvider.GetRestrictions().Returns(restrictions);

        var clientRestrictions = _clientRestrictionsProvider.GetClientRestrictions();

        var (sameBookItemCount, sameBookTimeUnitCount, sameBookTimeUnit) =
            clientRestrictions.BorrowedSameBookLimit;
        Assert.AreEqual(1, sameBookItemCount);
        Assert.AreEqual(restrictions.SameBookLimitDayCount, sameBookTimeUnitCount);
        Assert.AreEqual(TimeUnit.Day, sameBookTimeUnit);
    }

    [TestMethod]
    public void GetClientRestrictions_ShouldMapMaxExtensionsDaysPropertiesCorrectly()
    {
        var restrictions = new RawRestrictions()
        {
            MaxExtensionDays = 5,
            ExtensionDaysLimitMonthCount = 3
        };
        _restrictionsProvider.GetRestrictions().Returns(restrictions);

        var clientRestrictions = _clientRestrictionsProvider.GetClientRestrictions();

        var (extensionItemCount, extensionTimeUnitCount, extensionTimeUnit) =
            clientRestrictions.ExtensionDaysLimit;
        Assert.AreEqual(restrictions.MaxExtensionDays, extensionItemCount);
        Assert.AreEqual(restrictions.ExtensionDaysLimitMonthCount, extensionTimeUnitCount);
        Assert.AreEqual(TimeUnit.Month, extensionTimeUnit);
    }

    [TestMethod]
    public void GetClientRestrictions_ShouldMapSameDomainLimitPropertiesCorrectly()
    {
        var restrictions = new RawRestrictions()
        {
            MaxBorrowedBooksFromSameDomain = 20,
            SameDomainLimitMonthCount = 2
        };
        _restrictionsProvider.GetRestrictions().Returns(restrictions);

        var clientRestrictions = _clientRestrictionsProvider.GetClientRestrictions();

        var (sameDomainItemCount, sameDomainTimeUnitCount, sameDomainTimeUnit) =
            clientRestrictions.SameDomainBorrowedBooksLimit;
        Assert.AreEqual(restrictions.MaxBorrowedBooksFromSameDomain, sameDomainItemCount);
        Assert.AreEqual(restrictions.SameDomainLimitMonthCount, sameDomainTimeUnitCount);
        Assert.AreEqual(TimeUnit.Month, sameDomainTimeUnit);
    }

    [TestMethod]
    public void GetClientRestrictions_ShouldMapMaxBooksBorrowedAtOncePropertiesCorrectly()
    {
        var restrictions = new RawRestrictions()
        {
            MaxBorrowedBooksAtOnce = 10
        };
        _restrictionsProvider.GetRestrictions().Returns(restrictions);

        var clientRestrictions = _clientRestrictionsProvider.GetClientRestrictions();

        var maxBooksAtOnce = clientRestrictions.BorrowedBooksPerRequestLimit.ItemCount;
        Assert.AreEqual(restrictions.MaxBorrowedBooksAtOnce, maxBooksAtOnce);
    }

    [TestMethod]
    public void GetClientRestrictions_ShouldMapPerPeriodPropertiesCorrectly()
    {
        var restrictions = new RawRestrictions()
        {
            MaxBorrowedBooksPerPeriod = 10,
            PerPeriodLimitDayCount = 21,
        };
        _restrictionsProvider.GetRestrictions().Returns(restrictions);

        var clientRestrictions = _clientRestrictionsProvider.GetClientRestrictions();

        var (itemCount, timeUnitCount, timeUnit) = clientRestrictions.BorrowedBooksLimit;
        Assert.AreEqual(restrictions.MaxBorrowedBooksPerPeriod, itemCount);
        Assert.AreEqual(restrictions.PerPeriodLimitDayCount, timeUnitCount);
        Assert.AreEqual(TimeUnit.Day, timeUnit);
    }

    // big one
    [TestMethod]
    public void GetClientRestrictions_ShouldReturnCorrectRestrictions()
    {
        var restrictions = new RawRestrictions()
        {
            MaxBorrowedBooksPerPeriod = 30,
            PerPeriodLimitDayCount = 12,
            MaxBorrowedBooksAtOnce = 4,
            MaxBorrowedBooksFromSameDomain = 15,
            SameDomainLimitMonthCount = 3,
            MaxExtensionDays = 6,
            ExtensionDaysLimitMonthCount = 4,
            SameBookLimitDayCount = 4,
            MaxBorrowedBooksPerDay = 10
        };
        _restrictionsProvider.GetRestrictions().Returns(restrictions);

        var clientRestrictions = _clientRestrictionsProvider.GetClientRestrictions();

        var (itemCount, timeUnitCount, timeUnit) = clientRestrictions.BorrowedBooksLimit;
        Assert.AreEqual(restrictions.MaxBorrowedBooksPerPeriod, itemCount);
        Assert.AreEqual(restrictions.PerPeriodLimitDayCount, timeUnitCount);
        Assert.AreEqual(TimeUnit.Day, timeUnit);
        var maxBooksAtOnce = clientRestrictions.BorrowedBooksPerRequestLimit.ItemCount;
        Assert.AreEqual(restrictions.MaxBorrowedBooksAtOnce, maxBooksAtOnce);
        var (sameDomainItemCount, sameDomainTimeUnitCount, sameDomainTimeUnit) =
            clientRestrictions.SameDomainBorrowedBooksLimit;
        Assert.AreEqual(restrictions.MaxBorrowedBooksFromSameDomain, sameDomainItemCount);
        Assert.AreEqual(restrictions.SameDomainLimitMonthCount, sameDomainTimeUnitCount);
        Assert.AreEqual(TimeUnit.Month, sameDomainTimeUnit);
        var (extensionItemCount, extensionTimeUnitCount, extensionTimeUnit) =
            clientRestrictions.ExtensionDaysLimit;
        Assert.AreEqual(restrictions.MaxExtensionDays, extensionItemCount);
        Assert.AreEqual(restrictions.ExtensionDaysLimitMonthCount, extensionTimeUnitCount);
        Assert.AreEqual(TimeUnit.Month, extensionTimeUnit);
        var (sameBookItemCount, sameBookTimeUnitCount, sameBookTimeUnit) =
            clientRestrictions.BorrowedSameBookLimit;
        Assert.AreEqual(1, sameBookItemCount);
        Assert.AreEqual(restrictions.SameBookLimitDayCount, sameBookTimeUnitCount);
        Assert.AreEqual(TimeUnit.Day, sameBookTimeUnit);
        var perDayLimit = clientRestrictions.BorrowedBooksPerDayLimit.ItemCount;
        Assert.AreEqual(restrictions.MaxBorrowedBooksPerDay, perDayLimit);
    }

    //------------------------------------------------------------------------
    [TestMethod]
    public void GetPrivilegedClientRestrictions_ShouldMapMaxBooksBorrowedPerDayPropertiesCorrectly()
    {
        var restrictions = new RawRestrictions()
        {
            MaxBorrowedBooksPerDay = 6
        };
        _restrictionsProvider.GetRestrictions().Returns(restrictions);

        var clientRestrictions = _clientRestrictionsProvider.GetPrivilegedClientRestrictions();

        var perDayLimit = clientRestrictions.BorrowedBooksPerDayLimit.ItemCount;
        Assert.AreEqual(int.MaxValue, perDayLimit);
    }

    [TestMethod]
    public void GetPrivilegedClientRestrictions_ShouldMapSameBookLimitPropertiesCorrectly()
    {
        var restrictions = new RawRestrictions()
        {
            SameBookLimitDayCount = 10
        };
        _restrictionsProvider.GetRestrictions().Returns(restrictions);

        var clientRestrictions = _clientRestrictionsProvider.GetPrivilegedClientRestrictions();

        var (sameBookItemCount, sameBookTimeUnitCount, sameBookTimeUnit) =
            clientRestrictions.BorrowedSameBookLimit;
        Assert.AreEqual(1, sameBookItemCount);
        Assert.AreEqual(restrictions.SameBookLimitDayCount / 2, sameBookTimeUnitCount);
        Assert.AreEqual(TimeUnit.Day, sameBookTimeUnit);
    }

    [TestMethod]
    public void GetPrivilegedClientRestrictions_ShouldMapMaxExtensionsDaysPropertiesCorrectly()
    {
        var restrictions = new RawRestrictions()
        {
            MaxExtensionDays = 5,
            ExtensionDaysLimitMonthCount = 3
        };
        _restrictionsProvider.GetRestrictions().Returns(restrictions);

        var clientRestrictions = _clientRestrictionsProvider.GetPrivilegedClientRestrictions();

        var (extensionItemCount, extensionTimeUnitCount, extensionTimeUnit) =
            clientRestrictions.ExtensionDaysLimit;
        Assert.AreEqual(restrictions.MaxExtensionDays * 2, extensionItemCount);
        Assert.AreEqual(restrictions.ExtensionDaysLimitMonthCount, extensionTimeUnitCount);
        Assert.AreEqual(TimeUnit.Month, extensionTimeUnit);
    }

    [TestMethod]
    public void GetPrivilegedClientRestrictions_ShouldMapSameDomainLimitPropertiesCorrectly()
    {
        var restrictions = new RawRestrictions()
        {
            MaxBorrowedBooksFromSameDomain = 20,
            SameDomainLimitMonthCount = 2
        };
        _restrictionsProvider.GetRestrictions().Returns(restrictions);

        var clientRestrictions = _clientRestrictionsProvider.GetPrivilegedClientRestrictions();

        var (sameDomainItemCount, sameDomainTimeUnitCount, sameDomainTimeUnit) =
            clientRestrictions.SameDomainBorrowedBooksLimit;
        Assert.AreEqual(restrictions.MaxBorrowedBooksFromSameDomain * 2, sameDomainItemCount);
        Assert.AreEqual(restrictions.SameDomainLimitMonthCount, sameDomainTimeUnitCount);
        Assert.AreEqual(TimeUnit.Month, sameDomainTimeUnit);
    }

    [TestMethod]
    public void GetPrivilegedClientRestrictions_ShouldMapMaxBooksBorrowedAtOncePropertiesCorrectly()
    {
        var restrictions = new RawRestrictions()
        {
            MaxBorrowedBooksAtOnce = 10
        };
        _restrictionsProvider.GetRestrictions().Returns(restrictions);

        var clientRestrictions = _clientRestrictionsProvider.GetPrivilegedClientRestrictions();

        var maxBooksAtOnce = clientRestrictions.BorrowedBooksPerRequestLimit.ItemCount;
        Assert.AreEqual(restrictions.MaxBorrowedBooksAtOnce * 2, maxBooksAtOnce);
    }

    [TestMethod]
    public void GetPrivilegedClientRestrictions_ShouldMapPerPeriodPropertiesCorrectly()
    {
        var restrictions = new RawRestrictions()
        {
            MaxBorrowedBooksPerPeriod = 10,
            PerPeriodLimitDayCount = 21,
        };
        _restrictionsProvider.GetRestrictions().Returns(restrictions);

        var clientRestrictions = _clientRestrictionsProvider.GetPrivilegedClientRestrictions();

        var (itemCount, timeUnitCount, timeUnit) = clientRestrictions.BorrowedBooksLimit;
        Assert.AreEqual(restrictions.MaxBorrowedBooksPerPeriod * 2, itemCount);
        Assert.AreEqual(restrictions.PerPeriodLimitDayCount / 2, timeUnitCount);
        Assert.AreEqual(TimeUnit.Day, timeUnit);
    }

    [TestMethod]
    public void GetPrivilegedClientRestrictions_ShouldReturnCorrectRestrictions()
    {
        var restrictions = new RawRestrictions()
        {
            MaxBorrowedBooksPerPeriod = 40,
            PerPeriodLimitDayCount = 3,
            MaxBorrowedBooksAtOnce = 10,
            MaxBorrowedBooksFromSameDomain = 10,
            SameDomainLimitMonthCount = 4,
            MaxExtensionDays = 2,
            ExtensionDaysLimitMonthCount = 4,
            SameBookLimitDayCount = 6,
            MaxBorrowedBooksPerDay = 1
        };
        _restrictionsProvider.GetRestrictions().Returns(restrictions);

        var clientRestrictions = _clientRestrictionsProvider.GetPrivilegedClientRestrictions();

        var (itemCount, timeUnitCount, timeUnit) = clientRestrictions.BorrowedBooksLimit;
        Assert.AreEqual(restrictions.MaxBorrowedBooksPerPeriod * 2, itemCount);
        Assert.AreEqual(restrictions.PerPeriodLimitDayCount / 2, timeUnitCount);
        Assert.AreEqual(TimeUnit.Day, timeUnit);
        var maxBooksAtOnce = clientRestrictions.BorrowedBooksPerRequestLimit.ItemCount;
        Assert.AreEqual(restrictions.MaxBorrowedBooksAtOnce * 2, maxBooksAtOnce);
        var (sameDomainItemCount, sameDomainTimeUnitCount, sameDomainTimeUnit) =
            clientRestrictions.SameDomainBorrowedBooksLimit;
        Assert.AreEqual(restrictions.MaxBorrowedBooksFromSameDomain * 2, sameDomainItemCount);
        Assert.AreEqual(restrictions.SameDomainLimitMonthCount, sameDomainTimeUnitCount);
        Assert.AreEqual(TimeUnit.Month, sameDomainTimeUnit);
        var (extensionItemCount, extensionTimeUnitCount, extensionTimeUnit) =
            clientRestrictions.ExtensionDaysLimit;
        Assert.AreEqual(restrictions.MaxExtensionDays * 2, extensionItemCount);
        Assert.AreEqual(restrictions.ExtensionDaysLimitMonthCount, extensionTimeUnitCount);
        Assert.AreEqual(TimeUnit.Month, extensionTimeUnit);
        var (sameBookItemCount, sameBookTimeUnitCount, sameBookTimeUnit) =
            clientRestrictions.BorrowedSameBookLimit;
        Assert.AreEqual(1, sameBookItemCount);
        Assert.AreEqual(restrictions.SameBookLimitDayCount / 2, sameBookTimeUnitCount);
        Assert.AreEqual(TimeUnit.Day, sameBookTimeUnit);
        var perDayLimit = clientRestrictions.BorrowedBooksPerDayLimit.ItemCount;
        Assert.AreEqual(int.MaxValue, perDayLimit);
    }
}