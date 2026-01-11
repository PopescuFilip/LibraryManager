using System.Collections.Immutable;

namespace DomainModel.DTOs;

public record BookDetails(
    int BookId,
    Book Book,
    int BookEditionId,
    int BooksAvailable,
    int BooksTotal,
    ImmutableArray<int> DomainIds);