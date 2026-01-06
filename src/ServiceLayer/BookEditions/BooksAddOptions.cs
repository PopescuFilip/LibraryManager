namespace ServiceLayer.BookEditions;

public record BooksAddOptions(
    int ForReadingRoomCount,
    int ForBorrowingCount,
    int BookEditionId);