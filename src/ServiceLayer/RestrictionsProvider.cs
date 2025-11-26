using DataMapper;
using DomainModel.Restrictions;

namespace ServiceLayer;

public interface IRestrictionsProvider
{
    ClientRestrictions GetClientRestrictions();
    ClientRestrictions GetPrivilegedClientRestrictions();
}

public class RestrictionsProvider : IRestrictionsProvider
{
    private const int ExtensionDaysLimitMonthCount = 3;

    public ClientRestrictions GetClientRestrictions()
    {
        var rawRestrictions = AppSettings.Restrictions;

        var periodLimit = new PerDayLimit(rawRestrictions.MaxBorrowedBooksPerPeriod, rawRestrictions.PerPeriodLimitDayCount);
        var sameDomainLimit = new PerMonthLimit(rawRestrictions.MaxBorrowedBooksFromSameDomain, rawRestrictions.SameDomainLimitMonthCount);
        var extensionLimit = new PerMonthLimit(rawRestrictions.MaxExtensionDays, ExtensionDaysLimitMonthCount);
        var sameBookLimit = new PerDayLimit(1, rawRestrictions.SameBookLimitDayCount);

        return new ClientRestrictions(
            BorrowedBooksLimit: periodLimit,
            MaxBorrowedBooksAtOnce: rawRestrictions.MaxBorrowedBooksAtOnce,
            SameDomainBorrowedBooksLimit: sameDomainLimit,
            ExtensionDaysLimit: extensionLimit,
            BorrowedSameBookLimit: sameBookLimit,
            MaxBorrowedBooksPerDay: rawRestrictions.MaxBorrowedBooksPerDay
            );
    }

    public ClientRestrictions GetPrivilegedClientRestrictions()
    {
        var basicRestrictions = GetClientRestrictions();
        var privilegedClientRestrictions = basicRestrictions with
        {
        };

        return privilegedClientRestrictions;
    }
}