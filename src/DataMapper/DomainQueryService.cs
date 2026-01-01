using Microsoft.EntityFrameworkCore;

namespace DataMapper;

public interface IDomainQueryService
{
    int? GetIdByName(string name);

    IEnumerable<string> GetImplicitDomainNames(int id);
}

public class DomainQueryService(LibraryDbContext _context)
    : IDomainQueryService
{
    public int? GetIdByName(string name)
    {
        var foundIds = _context.Domains
            .AsNoTracking()
            .Where(d => d.Name == name)
            .Select(d => d.Id)
            .Take(2)
            .ToList();

        if (foundIds.Count == 2)
            throw new InvalidOperationException($"Found more than one Domain with name: {name}");

        return foundIds.Count == 0 ? null : foundIds.First();
    }

    public IEnumerable<string> GetImplicitDomainNames(int id)
    {
        var allDomains = _context.Domains
            .Include(d => d.ParentDomain)
            .ToList();

        var currentDomain = allDomains.FirstOrDefault(d => d.Id == id);

        while (currentDomain is not null)
        {
            yield return currentDomain.Name;
            currentDomain = currentDomain.ParentDomain;
        }
    }
}