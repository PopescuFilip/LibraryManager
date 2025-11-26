using DataMapper;
using DomainModel.Restrictions;
using Microsoft.Extensions.Configuration;

namespace ServiceLayer;

public interface IRestrictionsProvider
{
    ClientRestrictions GetClientRestrictions();
    ClientRestrictions GetPrivilegedClientRestrictions();
}

public class RestrictionsProvider(IConfiguration configuration) : IRestrictionsProvider
{
    private const string RestrictionsSection = "Restrictions";
    private const int ExtensionDaysLimitMonthCount = 3;

    private readonly Restrictions _restrictions =
        configuration.GetRequiredSection(RestrictionsSection).Get<Restrictions>()!;

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

        };

        return privilegedClientRestrictions;
    }
}