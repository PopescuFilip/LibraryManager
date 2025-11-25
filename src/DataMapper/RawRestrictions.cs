using DomainModel.Restrictions;
using Microsoft.Extensions.Configuration;

namespace DataMapper;

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

    [ConfigurationKeyName("DELTA")]
    public int SameBookLimitDayCount { get; init; }

    [ConfigurationKeyName("NCZ")]
    public int MaxBorrowedBooksPerDay { get; init; }

    [ConfigurationKeyName("PERSIMP")]
    public int MaxBorrowedBooksGivenPerDay { get; init; }

    public ClientRestrictions ToClientRestrictions() => new(
        MaxBorrowedBooksPerPeriod: MaxBorrowedBooksPerPeriod,
        PeriodInDays: PerPeriodLimitDayCount,
        MaxBorrowedBooksAtOnce: MaxBorrowedBooksAtOnce,
        MaxBorrowedBooksFromSameDomain: MaxBorrowedBooksFromSameDomain,
        SameDomainLimitMonthCount: SameDomainLimitMonthCount,
        MaxExtensionDays: MaxExtensionDays,
        MinDaysIntervalForSameBook: SameBookLimitDayCount,
        MaxBorrowedBooksPerDay: MaxBorrowedBooksPerDay
        );

    public EmployeeRestrictions ToEmployeeRestrictions() => new(MaxBorrowedBooksGivenPerDay);

    public BookRestrictions ToBookRestrictions() => new(MaxDomains);

    public override string ToString() =>
        $"""
        RawRestrictions:
        MaxDomains = {MaxDomains}
        MaxBorrowedBooksPerPeriod = {MaxBorrowedBooksPerPeriod}
        Period = {PerPeriodLimitDayCount}
        MaxBorrowedBooksAtOnce = {MaxBorrowedBooksAtOnce}
        MaxBorrowedBooksFromSameDomain = {MaxBorrowedBooksFromSameDomain}
        SameDomainLimitMonthCount = {SameDomainLimitMonthCount}
        MaxExtensionDays = {MaxExtensionDays}
        MinDaysIntervalForSameBook = {SameBookLimitDayCount}
        MaxBorrowedBooksPerDay = {MaxBorrowedBooksPerDay}
        MaxBorrowedBooksGivenPerDay = {MaxBorrowedBooksGivenPerDay}
        """;
}