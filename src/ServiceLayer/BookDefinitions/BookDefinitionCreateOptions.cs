namespace ServiceLayer.BookDefinitions;

public record BookDefinitionCreateOptions(
    string Name,
    IdCollection AuthorIds,
    IdCollection DomainIds);