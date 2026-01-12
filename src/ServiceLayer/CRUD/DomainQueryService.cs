using DataMapper;
using DataMapper.QueryHelpers;
using DomainModel;
using ServiceLayer.Exceptions;

namespace ServiceLayer.CRUD;

public interface IDomainQueryService
{
    int? GetIdByName(string name);

    IEnumerable<string> GetImplicitDomainNames(IEnumerable<int> ids);

    IEnumerable<int> GetParentIds(IEnumerable<int> ids);

    IEnumerable<BookParentDomainIds> GetParentDomainIds(IEnumerable<BookDomainIds> ids);
}

public class DomainQueryService(IRepository<Domain> _repository)
    : IDomainQueryService
{
    public int? GetIdByName(string name)
    {
        var foundIds = _repository.Get(
            select: Select<Domain>.Id,
            collector: q => q.Take(2).ToList(),
            filter: d => d.Name == name,
            orderBy: Order<Domain>.ById,
            asNoTracking: true
            );

        if (foundIds.Count == 2)
            throw new DuplicateDomainNameException(name);

        return foundIds.Count == 0 ? null : foundIds.First();
    }

    public IEnumerable<string> GetImplicitDomainNames(IEnumerable<int> ids)
    {
        var allDomains = GetAllDomains();

        return ids.Distinct().SelectMany(id => GetImplicitDomainNames(allDomains, id));
    }

    public IEnumerable<int> GetParentIds(IEnumerable<int> ids)
    {
        var allDomains = GetAllDomains();

        return ids.Distinct().Select(id => GetParentId(allDomains, id));
    }

    public IEnumerable<BookParentDomainIds> GetParentDomainIds(IEnumerable<BookDomainIds> ids)
    {
        var allDomains = GetAllDomains();
        var cachedParentDomainIds = new Dictionary<int, int>();

        foreach (var bookDomainIds in ids)
        {
            var bookParentDomainIds = new List<int>();

            foreach (var domainId in bookDomainIds)
            {
                if (!cachedParentDomainIds.TryGetValue(domainId, out int parentId))
                {
                    parentId = GetParentId(allDomains, domainId);
                    cachedParentDomainIds[domainId] = parentId;
                }
                bookParentDomainIds.Add(parentId);
            }

            yield return new BookParentDomainIds(bookParentDomainIds);
        }
    }

    private static IEnumerable<string> GetImplicitDomainNames(IReadOnlyCollection<Domain> domains, int id) =>
        GetImplicitDomains(domains, id).Select(x => x.Name);

    private static int GetParentId(IReadOnlyCollection<Domain> domains, int id) =>

        GetImplicitDomains(domains, id, true).Select(x => x.Id).Last();


    private static IEnumerable<Domain> GetImplicitDomains(IReadOnlyCollection<Domain> domains, int id, bool includeCurrent = false)
    {
        var currentDomain = domains.FirstOrDefault(d => d.Id == id);
        if (currentDomain is null)
            yield break;

        if (includeCurrent)
            yield return currentDomain;

        while (currentDomain.ParentDomain is not null)
        {
            yield return currentDomain.ParentDomain;
            currentDomain = currentDomain.ParentDomain;
        }
    }

    private IReadOnlyCollection<Domain> GetAllDomains() =>
        _repository.Get(
            select: Select<Domain>.Default,
            collector: Collector<Domain>.ToList,
            asNoTracking: false,
            orderBy: Order<Domain>.ById,
            includeProperties: d => d.ParentDomain
            );
}