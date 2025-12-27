using Microsoft.EntityFrameworkCore;

namespace DataMapper;

public interface IDomainQueryService
{
    int? GetIdByName(string name);

    IEnumerable<string> GetImplicitDomainNames(int id);
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

    public IEnumerable<string> GetImplicitDomainNames(int id)
    {
        using var context = _dbContextFactory.CreateDbContext();

        var allDomains = context.Domains
            .Include(d => d.ParentDomain)
            .ToList();

        var currentDomain = allDomains.First(d => d.Id == id);
        yield return currentDomain.Name;

        while (currentDomain.ParentDomain is not null)
        {
            yield return currentDomain.ParentDomain.Name;
            currentDomain = currentDomain.ParentDomain;
        }
    }
}