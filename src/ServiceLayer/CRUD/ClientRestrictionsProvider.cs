using DataMapper;
using DomainModel.Restrictions;

namespace ServiceLayer.CRUD;

public interface IClientRestrictionsProvider
{
    Result<ClientRestrictions> GetClientRestrictions();
    Result<ClientRestrictions> GetPrivilegedClientRestrictions();
}

public class ClientRestrictionsProvider(IRestrictionsProvider _restrictionsProvider) : IClientRestrictionsProvider
{
    public Result<ClientRestrictions> GetClientRestrictions()
    {
        var restrictions = _restrictionsProvider.GetRestrictions();
        if (restrictions is null)
            return Result.Invalid();

        var periodLimit = Limit.PerPeriodInDays(restrictions.MaxBorrowedBooksPerPeriod, restrictions.PerPeriodLimitDayCount);
        var borrowedBooksAtOnceLimit = Limit.PerRequest(restrictions.MaxBorrowedBooksAtOnce);
        var sameDomainLimit = Limit.PerPeriodInMonths(restrictions.MaxBorrowedBooksFromSameDomain, restrictions.SameDomainLimitMonthCount);
        var extensionLimit = Limit.PerPeriodInMonths(restrictions.MaxExtensionDays, restrictions.ExtensionDaysLimitMonthCount);
        var sameBookLimit = Limit.PerPeriodInDays(1, restrictions.SameBookLimitDayCount);
        var borrowedBooksPerDayLimit = Limit.PerDay(restrictions.MaxBorrowedBooksPerDay);

        var clientRestrictions = new ClientRestrictions (
            BorrowedBooksLimit: periodLimit,
            BorrowedBooksPerRequestLimit: borrowedBooksAtOnceLimit,
            SameDomainBorrowedBooksLimit: sameDomainLimit,
            ExtensionDaysLimit: extensionLimit,
            BorrowedSameBookLimit: sameBookLimit,
            BorrowedBooksPerDayLimit: borrowedBooksPerDayLimit
            );

        return Result.Valid(clientRestrictions);
    }

    public Result<ClientRestrictions> GetPrivilegedClientRestrictions()
    {
        var result = GetClientRestrictions();
        if (!result.IsValid)
            return Result.Invalid();

        var basicRestrictions = result.Get();
        var privilegedClientRestrictions = basicRestrictions with
        {
            BorrowedBooksLimit = basicRestrictions.BorrowedBooksLimit.DoubleItem().HalfTime(),
            BorrowedBooksPerRequestLimit = basicRestrictions.BorrowedBooksPerRequestLimit.DoubleItem(),
            SameDomainBorrowedBooksLimit = basicRestrictions.SameDomainBorrowedBooksLimit.DoubleItem(),
            ExtensionDaysLimit = basicRestrictions.ExtensionDaysLimit.DoubleItem(),
            BorrowedSameBookLimit = basicRestrictions.BorrowedSameBookLimit.HalfTime(),
            BorrowedBooksPerDayLimit = Limit.PerDay(int.MaxValue)
        };

        return Result.Valid(privilegedClientRestrictions);
    }
}