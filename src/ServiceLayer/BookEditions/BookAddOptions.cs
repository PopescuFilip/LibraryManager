namespace ServiceLayer.BookEditions;

public record BookAddOptions(
    int ForReadingRoomCount,
    int ForBorrowingCount,
    int BookEditionId);