namespace DomainModel;

public static class Extensions
{
    public static IReadOnlyDictionary<BookStatus, int> Substract(
        this IReadOnlyDictionary<BookStatus, int> current,
        IReadOnlyDictionary<BookStatus, int> other) =>
        current.Keys.Intersect(other.Keys)
        .Select(key => new { Status = key, Count = current[key] - other[key] })
        .ToDictionary(x => x.Status, x => x.Count);

    public static bool MatchesPerfectly(this IEnumerable<Book> books,
        IReadOnlyDictionary<BookStatus, int> countForStatus) =>
        countForStatus.Values.Sum() == books.Count()
        && books.Matches(countForStatus);

    public static bool Matches(this IEnumerable<Book> books,
        IReadOnlyDictionary<BookStatus, int> countForStatus) =>
        countForStatus.All(kvp => books.Count(x => x.Status == kvp.Key) == kvp.Value);
}