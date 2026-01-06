using DomainModel;

namespace ServiceLayer.BookEditions;

public record BooksUpdateOptions(
    int ForReadingRoomCount,
    int ForBorrowingCount,
    int BookEditionId);

public static class BooksUpdateOptionsExtensions
{
    public static IReadOnlyDictionary<BookStatus, int> ToStatusCountDictionary(
        this BooksUpdateOptions options) =>
        new Dictionary<BookStatus, int>()
        {
            { BookStatus.ForReadingRoom, options.ForReadingRoomCount },
            { BookStatus.Available, options.ForBorrowingCount }
        };
}