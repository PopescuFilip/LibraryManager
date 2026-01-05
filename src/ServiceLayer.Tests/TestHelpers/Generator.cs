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
}