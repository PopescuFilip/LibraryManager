namespace DomainModel.Restrictions;

public record class ClientRestrictions(
    PeriodLimit BorrowedBooksLimit,
    PerRequestLimit BorrowedBooksPerRequestLimit,
    PeriodLimit SameDomainBorrowedBooksLimit,
    PeriodLimit ExtensionDaysLimit,
    PeriodLimit BorrowedSameBookLimit,
    PerDayLimit BorrowedBooksPerDayLimit);