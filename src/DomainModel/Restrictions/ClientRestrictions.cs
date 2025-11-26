namespace DomainModel.Restrictions;

public record class ClientRestrictions(
    PerDayLimit BorrowedBooksLimit,
    int MaxBorrowedBooksAtOnce,
    PerMonthLimit SameDomainBorrowedBooksLimit,
    PerMonthLimit ExtensionDaysLimit,
    PerDayLimit BorrowedSameBookLimit,
    int MaxBorrowedBooksPerDay);