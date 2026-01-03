using DataMapper;
using DomainModel.Restrictions;

namespace ServiceLayer.CRUD;

public interface IClientRestrictionsProvider
{
    ClientRestrictions GetClientRestrictions();
    ClientRestrictions GetPrivilegedClientRestrictions();
}

public class ClientRestrictionsProvider(IRestrictionsProvider _restrictionsProvider) : IClientRestrictionsProvider
{
    private const int ExtensionDaysLimitMonthCount = 3;

    public ClientRestrictions GetClientRestrictions()
    {
        var restrictions = _restrictionsProvider.GetRestrictions()!;

        var periodLimit = Limit.PerDay(restrictions.MaxBorrowedBooksPerPeriod, restrictions.PerPeriodLimitDayCount);
        var sameDomainLimit = Limit.PerMonth(restrictions.MaxBorrowedBooksFromSameDomain, restrictions.SameDomainLimitMonthCount);
        var extensionLimit = Limit.PerMonth(restrictions.MaxExtensionDays, ExtensionDaysLimitMonthCount);
        var sameBookLimit = Limit.PerDay(1, restrictions.SameBookLimitDayCount);

        return new ClientRestrictions(
            BorrowedBooksLimit: periodLimit,
            MaxBorrowedBooksAtOnce: restrictions.MaxBorrowedBooksAtOnce,
            SameDomainBorrowedBooksLimit: sameDomainLimit,
            ExtensionDaysLimit: extensionLimit,
            BorrowedSameBookLimit: sameBookLimit,
            MaxBorrowedBooksPerDay: restrictions.MaxBorrowedBooksPerDay
            );
    }

    public ClientRestrictions GetPrivilegedClientRestrictions()
    {
        var basicRestrictions = GetClientRestrictions();
        var privilegedClientRestrictions = basicRestrictions with
        {
            BorrowedBooksLimit = basicRestrictions.BorrowedBooksLimit.DoubleItem().HalfTime(),
            MaxBorrowedBooksAtOnce = basicRestrictions.MaxBorrowedBooksAtOnce * 2,
            SameDomainBorrowedBooksLimit = basicRestrictions.SameDomainBorrowedBooksLimit.DoubleItem(),
            ExtensionDaysLimit = basicRestrictions.ExtensionDaysLimit.DoubleItem(),
            BorrowedSameBookLimit = basicRestrictions.BorrowedSameBookLimit.HalfTime(),
            MaxBorrowedBooksPerDay = int.MaxValue
        };

        return privilegedClientRestrictions;
    }
}