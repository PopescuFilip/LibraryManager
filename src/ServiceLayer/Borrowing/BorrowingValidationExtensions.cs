using DomainModel.DTOs;

namespace ServiceLayer.Borrowing;

public static class BorrowingValidationExtensions
{
    public static bool ContainsDuplicateBookEdition(this IReadOnlyCollection<BookDetails> bookDetails) =>
        bookDetails.Select(x => x.BookEditionId).Distinct().Count() != bookDetails.Count;

    public static IEnumerable<int> SelectDomainIds(this IReadOnlyCollection<BookDetails> bookDetails) =>
        bookDetails.SelectMany(x => x.DomainIds);
}