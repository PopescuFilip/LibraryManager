namespace DataMapper;

public class EmployeeRestrictions(RawRestrictions rawRestrictions)
{
    public int MaxDomains { get; init; } = rawRestrictions.MaxDomains;
    public int MaxBorrowedBooksPerPeriod { get; init; } = rawRestrictions.MaxBorrowedBooksPerPeriod * 2;
    public int PeriodInDays { get; init; } = rawRestrictions.PeriodInDays / 2;
    public int MaxBorrowedBooksAtOnce { get; init; } = rawRestrictions.MaxBorrowedBooksAtOnce * 2;
    public int MaxBorrowedBooksFromSameDomain { get; init; } = rawRestrictions.MaxBorrowedBooksFromSameDomain * 2;
    public int SameDomainLimitMonthCount { get; init; } = rawRestrictions.SameDomainLimitMonthCount;
    public int MaxExtensionDays { get; init; } = rawRestrictions.MaxExtensionDays * 2;
    public int MinDaysIntervalForSameBook { get; init; } = rawRestrictions.MinDaysIntervalForSameBook / 2;
    public int MaxBorrowedBooksPerDay { get; init; } = int.MaxValue;
    public int MaxBorrowedBooksGivenPerDay { get; init; } = rawRestrictions.MaxBorrowedBooksGivenPerDay;
}