namespace DomainModel.Restrictions;

public record class ClientRestrictions(
    int MaxBorrowedBooksPerPeriod,
    int PeriodInDays,
    int MaxBorrowedBooksAtOnce,
    int MaxBorrowedBooksFromSameDomain,
    int SameDomainLimitMonthCount,
    int MaxExtensionDays,
    int MinDaysIntervalForSameBook,
    int MaxBorrowedBooksPerDay);

public record class BetterClientRestrictions(
    PerDayLimit BorrowedBooksLimit,
    int MaxBorrowedBooksAtOnce,
    PerMonthLimit SameDomainBorrowedBooksLimit,
    PerMonthLimit ExtensionDaysLimit,
    PerDayLimit BorrowedSameBookLimit,
    PerDayLimit BorrowedBooksDailyLimit);