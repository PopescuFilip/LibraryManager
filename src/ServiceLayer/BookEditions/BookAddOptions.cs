namespace ServiceLayer.BookEditions;

public record BookAddOptions(
    int BooksForReadingRoomCount,
    int BooksForBorrowingCount,
    int BookEditionId);