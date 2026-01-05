using DomainModel;

namespace ServiceLayer.Domains;

public static class DomainHelpers
{
    public static readonly IEqualityComparer<Domain> ContainsComparer = EqualityComparer<Domain>.Create(
        (current, other) => current?.Name == other?.Name
                            || current?.Id == other?.Id
        );
}