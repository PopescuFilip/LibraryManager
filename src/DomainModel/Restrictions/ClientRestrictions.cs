namespace DomainModel.Restrictions;

public record class ClientRestrictions(
    int MaxDomains,
    int MaxBorrowedBooksPerPeriod,
    int PeriodInDays,
    int MaxBorrowedBooksAtOnce,
    int MaxBorrowedBooksFromSameDomain,
    int SameDomainLimitMonthCount,
    int MaxExtensionDays,
    int MinDaysIntervalForSameBook,
    int MaxBorrowedBooksPerDay);