namespace DomainModel.Restrictions;

public class ClientRestrictions
{
    public int MaxDomains { get; init; }
    public int MaxBorrowedBooksPerPeriod { get; init; }
    public int PeriodInDays { get; init; }
    public int MaxBorrowedBooksAtOnce { get; init; }
    public int MaxBorrowedBooksFromSameDomain { get; init; }
    public int SameDomainLimitMonthCount { get; init; }
    public int MaxExtensionDays { get; init; }
    public int MinDaysIntervalForSameBook { get; init; }
    public int MaxBorrowedBooksPerDay { get; init; }
}