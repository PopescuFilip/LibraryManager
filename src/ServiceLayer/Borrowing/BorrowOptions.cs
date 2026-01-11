namespace ServiceLayer.Borrowing;

public record BorrowOptions(int BookId, DateTime BorrowUntil);

public static class BorrowOptionsExtensions
{
    public static IdCollection ToIdCollection(this IEnumerable<BorrowOptions> borrowOptionsCollection) =>
        borrowOptionsCollection.Select(x => x.BookId).ToIdCollection();
}