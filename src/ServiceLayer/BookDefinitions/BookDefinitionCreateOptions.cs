using System.Collections.Immutable;

namespace ServiceLayer.BookDefinitions;

public record BookDefinitionCreateOptions(
    string Name,
    ImmutableArray<int> AuthorIds,
    ImmutableArray<int> DomainIds);