namespace DomainModel;

public static class Extensions
{
    public static bool MatchesPerfectly(this IEnumerable<Book> books,
        IReadOnlyDictionary<BookStatus, int> countForStatus) =>
        countForStatus.Values.Sum() == books.Count()
        && books.Matches(countForStatus);

    public static bool Matches(this IEnumerable<Book> books,
        IReadOnlyDictionary<BookStatus, int> countForStatus) =>
        countForStatus.All(kvp => books.Count(x => x.Status == kvp.Key) == kvp.Value);
}