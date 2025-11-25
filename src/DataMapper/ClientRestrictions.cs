namespace DataMapper;

public class ClientRestrictions(RawRestrictions rawRestrictions)
{
    public int MaxDomains { get; init; } = rawRestrictions.MaxDomains;
    public int MaxBorrowedBooksPerPeriod { get; init; } = rawRestrictions.MaxBorrowedBooksPerPeriod;
    public int PeriodInDays { get; init; } = rawRestrictions.PeriodInDays;
    public int MaxBorrowedBooksAtOnce { get; init; } = rawRestrictions.MaxBorrowedBooksAtOnce;
    public int MaxBorrowedBooksFromSameDomain { get; init; } = rawRestrictions.MaxBorrowedBooksFromSameDomain;
    public int SameDomainLimitMonthCount { get; init; } = rawRestrictions.SameDomainLimitMonthCount;
    public int MaxExtensionDays { get; init; } = rawRestrictions.MaxExtensionDays;
    public int MinDaysIntervalForSameBook { get; init; } = rawRestrictions.MinDaysIntervalForSameBook;
    public int MaxBorrowedBooksPerDay { get; init; } = rawRestrictions.MaxBorrowedBooksPerDay;
}