using DomainModel;
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
}