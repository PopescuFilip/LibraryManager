using DomainModel;
using ServiceLayer.Borrowing;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace ServiceLayer.UnitTests.TestHelpers;

[ExcludeFromCodeCoverage]
public static class Generator
{
    public static List<Domain> GenerateDomainsFrom(List<(int Id, string Name)> idsWithName) =>
        idsWithName
        .Select(kvp => new Domain(kvp.Name) { Id = kvp.Id })
        .ToList();

    public static List<Domain> GenerateDomainsFrom(IEnumerable<int> ids) =>
        ids.Select(id => new Domain("") { Id = id }).ToList();

    public static List<Author> GenerateAuthorsFrom(IEnumerable<int> ids) =>
        ids.Select(id => new Author("") { Id = id }).ToList();

    public static List<BookEdition> GenerateBookEditionsFrom(IEnumerable<int> ids) =>
        ids.Select(id => new BookEdition("", 21, BookType.BoardBook, 1) { Id = id }).ToList();

    public static List<Client> GenerateClientsFrom(List<(int Id, int AccountId)> idsWithAccounts) =>
        idsWithAccounts
        .Select(kvp => new Client(kvp.AccountId) { Id = kvp.Id })
        .ToList();

    public static List<Employee> GenerateEmployeesFrom(List<(int Id, int AccountId)> idsWithAccounts) =>
        idsWithAccounts
        .Select(kvp => new Employee(kvp.AccountId) { Id = kvp.Id })
        .ToList();

    public static List<BorrowRecord> GenerateBorrowRecordsForLender(IEnumerable<(int LenderId, int BookCount, DateTime Date)> borrowRecordCreateOptions) =>
        borrowRecordCreateOptions
        .SelectMany(opt => Enumerable.Range(0, opt.BookCount).Select(_ =>
            new BorrowRecord(321, opt.LenderId, 32, opt.Date.AddDays(1))
            {
                BorrowDateTime = opt.Date
            }))
        .ToList();

    public static List<BorrowRecord> GenerateBorrowRecordsForBorrower(IEnumerable<(int BorrowerId, int BookCount, DateTime Date)> borrowRecordCreateOptions) =>
        borrowRecordCreateOptions
        .SelectMany(opt => Enumerable.Range(0, opt.BookCount).Select(_ =>
            new BorrowRecord(opt.BorrowerId, 43, 32, opt.Date.AddDays(1))
            {
                BorrowDateTime = opt.Date
            }))
        .ToList();

    public static List<BorrowRecord> GenerateBorrowRecordsForBorrower(
        IEnumerable<(int BorrowerId, int EditionId, int BookCount, DateTime Date)> borrowRecordCreateOptions) =>
        borrowRecordCreateOptions
        .SelectMany(opt => Enumerable.Range(0, opt.BookCount).Select(_ =>
            new BorrowRecord(opt.BorrowerId, 43, 32, opt.Date.AddDays(1))
            {
                BorrowDateTime = opt.Date,
                BorrowedBook = new Book(BookStatus.Borrowed, opt.EditionId)
            }))
        .ToList();

    public static ImmutableArray<BorrowOptions> GenerateBorrowOptionsFrom(IEnumerable<int> ids, DateTime borrowUntil) =>
        ids.Select(id => new BorrowOptions(id, borrowUntil)).ToImmutableArray();
}