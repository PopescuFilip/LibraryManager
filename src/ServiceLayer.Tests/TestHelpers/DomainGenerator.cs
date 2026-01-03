using DomainModel;

namespace ServiceLayer.UnitTests.TestHelpers;

public static class DomainGenerator
{
    public static List<Domain> From(List<(int Id, string Name)> idsWithName) =>
        idsWithName
        .Select(kvp => new Domain(kvp.Name) { Id = kvp.Id })
        .ToList();
}