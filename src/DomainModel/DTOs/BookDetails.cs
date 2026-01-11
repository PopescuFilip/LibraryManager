using System.Collections.Immutable;

namespace DomainModel.DTOs;

public record BookDetails(int BookId, int BookEditionId, ImmutableArray<int> DomainIds);