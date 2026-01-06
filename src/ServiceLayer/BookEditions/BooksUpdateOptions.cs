namespace ServiceLayer.BookEditions;

public record BooksUpdateOptions(
    int ForReadingRoomCount,
    int ForBorrowingCount,
    int BookEditionId);