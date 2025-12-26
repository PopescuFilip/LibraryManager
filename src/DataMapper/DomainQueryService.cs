using Microsoft.EntityFrameworkCore;

namespace DataMapper;

public interface IDomainQueryService
{
    int? GetIdByName(string name);
}

public class DomainQueryService(IDbContextFactory<LibraryDbContext> _dbContextFactory)
    : IDomainQueryService
{
    public int? GetIdByName(string name)
    {
        using var context = _dbContextFactory.CreateDbContext();

        var foundIds = context.Domains
            .AsNoTracking()
            .Where(d =>  d.Name == name)
            .Select(d => d.Id)
            .Take(2)
            .ToList();

        if (foundIds.Count == 2)
            throw new InvalidOperationException($"Found more than one Domain with name: {name}");

        return foundIds.Count == 0 ? null : foundIds.First();
    }
}