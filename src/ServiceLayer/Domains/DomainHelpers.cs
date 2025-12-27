using DomainModel;

namespace ServiceLayer.Domains;

public static class DomainHelpers
{
    public static readonly IEqualityComparer<Domain> ContainsComparer = EqualityComparer<Domain>.Create(
        (current, other) => current?.Name == other?.Name
                            || current?.Id == other?.Id
        );

    public static IEnumerable<Domain> GetDomainsFlattened(this Domain domain, bool includeCurrent = false)
    {
        if (includeCurrent)
            yield return domain;

        foreach (var current in domain.SubDomains.SelectMany(s => s.GetDomainsFlattened(true)))
            yield return current;
    }
}