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
    public int PeriodInDays { get; init; }

    [ConfigurationKeyName("C")]
    public int MaxBorrowedBooksAtOnce { get; init; }

    [ConfigurationKeyName("D")]
    public int MaxBorrowedBooksFromSameDomain { get; init; }

    [ConfigurationKeyName("L")]
    public int SameDomainLimitMonthCount { get; init; }

    [ConfigurationKeyName("LIM")]
    public int MaxExtensionDays { get; init; }

    [ConfigurationKeyName("DELTA")]
    public int MinDaysIntervalForSameBook { get; init; }

    [ConfigurationKeyName("NCZ")]
    public int MaxBorrowedBooksPerDay { get; init; }

    [ConfigurationKeyName("PERSIMP")]
    public int MaxBorrowedBooksGivenPerDay { get; init; }

    public ClientRestrictions ToClientRestrictions() => new(
        MaxBorrowedBooksPerPeriod: MaxBorrowedBooksPerPeriod,
        PeriodInDays: PeriodInDays,
        MaxBorrowedBooksAtOnce: MaxBorrowedBooksAtOnce,
        MaxBorrowedBooksFromSameDomain: MaxBorrowedBooksFromSameDomain,
        SameDomainLimitMonthCount: SameDomainLimitMonthCount,
        MaxExtensionDays: MaxExtensionDays,
        MinDaysIntervalForSameBook: MinDaysIntervalForSameBook,
        MaxBorrowedBooksPerDay: MaxBorrowedBooksPerDay
        );

    public EmployeeRestrictions ToEmployeeRestrictions() => new(MaxBorrowedBooksPerDay);

    public BookRestrictions ToBookRestrictions() => new(MaxDomains);

    public override string ToString() =>
        $"""
        RawRestrictions:
        MaxDomains = {MaxDomains}
        MaxBorrowedBooksPerPeriod = {MaxBorrowedBooksPerPeriod}
        Period = {PeriodInDays}
        MaxBorrowedBooksAtOnce = {MaxBorrowedBooksAtOnce}
        MaxBorrowedBooksFromSameDomain = {MaxBorrowedBooksFromSameDomain}
        SameDomainLimitMonthCount = {SameDomainLimitMonthCount}
        MaxExtensionDays = {MaxExtensionDays}
        MinDaysIntervalForSameBook = {MinDaysIntervalForSameBook}
        MaxBorrowedBooksPerDay = {MaxBorrowedBooksPerDay}
        MaxBorrowedBooksGivenPerDay = {MaxBorrowedBooksGivenPerDay}
        """;
}