namespace DomainModel.Restrictions;

public record class ClientRestrictions(
    Limit BorrowedBooksLimit,
    int MaxBorrowedBooksAtOnce,
    Limit SameDomainBorrowedBooksLimit,
    Limit ExtensionDaysLimit,
    Limit BorrowedSameBookLimit,
    int MaxBorrowedBooksPerDay);