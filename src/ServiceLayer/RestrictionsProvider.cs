using DataMapper;
using DomainModel.Restrictions;

namespace ServiceLayer;

public interface IRestrictionsProvider
{
    BetterClientRestrictions GetClientRestrictions();
    BetterClientRestrictions GetPrivilegedClientRestrictions();
}

public class RestrictionsProvider : IRestrictionsProvider
{
    private const int ExtensionDaysLimitMonthCount = 3;

    public BetterClientRestrictions GetClientRestrictions()
    {
        var rawRestrictions = AppSettings.Restrictions;

        var periodLimit = new PerDayLimit(rawRestrictions.MaxBorrowedBooksPerPeriod, rawRestrictions.PerPeriodLimitDayCount);
        var sameDomainLimit = new PerMonthLimit(rawRestrictions.MaxBorrowedBooksFromSameDomain, rawRestrictions.SameDomainLimitMonthCount);
        var extensionLimit = new PerMonthLimit(rawRestrictions.MaxExtensionDays, ExtensionDaysLimitMonthCount);
        var sameBookLimit = new PerDayLimit(1, rawRestrictions.SameBookLimitDayCount);
        var dailyLimit = new PerDayLimit(rawRestrictions.MaxBorrowedBooksPerDay, 1);

        return new BetterClientRestrictions(
            BorrowedBooksLimit: periodLimit,
            MaxBorrowedBooksAtOnce: rawRestrictions.MaxBorrowedBooksAtOnce,
            SameDomainBorrowedBooksLimit: sameDomainLimit,
            ExtensionDaysLimit: extensionLimit,
            BorrowedSameBookLimit: sameBookLimit,
            BorrowedBooksDailyLimit: dailyLimit
            );
    }

    public BetterClientRestrictions GetPrivilegedClientRestrictions()
    {
        var basicRestrictions = GetClientRestrictions();
        var privilegedClientRestrictions = basicRestrictions with
        {
        };

        return privilegedClientRestrictions;
    }
}