using DataMapper;
using DomainModel.Restrictions;

namespace ServiceLayer.CRUD;

public interface IClientRestrictionsProvider
{
    ClientRestrictions GetClientRestrictions();
    ClientRestrictions GetPrivilegedClientRestrictions();
}

public class ClientRestrictionsProvider(IRestrictionsProvider restrictionsProvider) : IClientRestrictionsProvider
{
    private const int ExtensionDaysLimitMonthCount = 3;

    private readonly Restrictions _restrictions = restrictionsProvider.GetRestrictions()!;

    public ClientRestrictions GetClientRestrictions()
    {
        var periodLimit = Limit.PerDay(_restrictions.MaxBorrowedBooksPerPeriod, _restrictions.PerPeriodLimitDayCount);
        var sameDomainLimit = Limit.PerMonth(_restrictions.MaxBorrowedBooksFromSameDomain, _restrictions.SameDomainLimitMonthCount);
        var extensionLimit = Limit.PerMonth(_restrictions.MaxExtensionDays, ExtensionDaysLimitMonthCount);
        var sameBookLimit = Limit.PerDay(1, _restrictions.SameBookLimitDayCount);

        return new ClientRestrictions(
            BorrowedBooksLimit: periodLimit,
            MaxBorrowedBooksAtOnce: _restrictions.MaxBorrowedBooksAtOnce,
            SameDomainBorrowedBooksLimit: sameDomainLimit,
            ExtensionDaysLimit: extensionLimit,
            BorrowedSameBookLimit: sameBookLimit,
            MaxBorrowedBooksPerDay: _restrictions.MaxBorrowedBooksPerDay
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