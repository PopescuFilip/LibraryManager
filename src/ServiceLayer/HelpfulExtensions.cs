using DomainModel;
using ServiceLayer.BookEditions;

namespace ServiceLayer;

public static class HelpfulExtensions
{
    public static BooksUpdateOptions Substract(
        this BooksUpdateOptions current,
        BooksUpdateOptions other) =>
        new(
            current.ForReadingRoomCount - other.ForReadingRoomCount,
            current.ForBorrowingCount - other.ForBorrowingCount,
            current.BookEditionId);

    public static bool MatchesPerfectly(this IEnumerable<Book> books,
        BooksUpdateOptions options) =>
        books.Count() == options.ForBorrowingCount + options.ForReadingRoomCount
        && books.Matches(options);

    public static bool Matches(this IEnumerable<Book> books, BooksUpdateOptions options) =>
        books.Matches(options.ToStatusCountDictionary());
}