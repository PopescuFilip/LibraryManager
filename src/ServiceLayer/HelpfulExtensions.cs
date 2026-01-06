using DomainModel;
using Microsoft.Extensions.Options;
using ServiceLayer.BookEditions;
using static System.Reflection.Metadata.BlobBuilder;

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
        options
        .ToStatusCountDictionary()
        .All(kvp => books.Count(x => x.Status == kvp.Key) == kvp.Value);
}