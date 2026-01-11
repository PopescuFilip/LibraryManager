using Microsoft.Extensions.Configuration;

namespace DomainModel.Restrictions;

public class RawRestrictions
{
    [ConfigurationKeyName("DOMENII")]
    public int MaxDomains { get; init; }

    [ConfigurationKeyName("NMC")]
    public int MaxBorrowedBooksPerPeriod { get; init; }

    [ConfigurationKeyName("PER")]
    public int PerPeriodLimitDayCount { get; init; }

    [ConfigurationKeyName("C")]
    public int MaxBorrowedBooksAtOnce { get; init; }

    [ConfigurationKeyName("D")]
    public int MaxBorrowedBooksFromSameDomain { get; init; }

    [ConfigurationKeyName("L")]
    public int SameDomainLimitMonthCount { get; init; }

    [ConfigurationKeyName("LIM")]
    public int MaxExtensionDays { get; init; }

    public int ExtensionDaysLimitMonthCount { get; init; } = 3;

    [ConfigurationKeyName("DELTA")]
    public int SameBookLimitDayCount { get; init; }

    [ConfigurationKeyName("NCZ")]
    public int MaxBorrowedBooksPerDay { get; init; }

    [ConfigurationKeyName("PERSIMP")]
    public int MaxBorrowedBooksGivenPerDay { get; init; }
}