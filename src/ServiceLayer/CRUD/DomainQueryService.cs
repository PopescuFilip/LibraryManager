using DataMapper;
using DataMapper.QueryHelpers;
using DomainModel;
using ServiceLayer.Exceptions;

namespace ServiceLayer.CRUD;

public interface IDomainQueryService
{
    int? GetIdByName(string name);

    IEnumerable<string> GetImplicitDomainNames(IEnumerable<int> ids);
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
        var allDomains = _repository.Get(
            select: Select<Domain>.Default,
            collector: Collector<Domain>.ToList,
            asNoTracking: false,
            orderBy: Order<Domain>.ById,
            includeProperties: d => d.ParentDomain
            );

        return ids.SelectMany(id => GetImplicitDomainNames(allDomains, id));
    }

    private static IEnumerable<string> GetImplicitDomainNames(IReadOnlyCollection<Domain> domains, int id)
    {
        var currentDomain = domains.FirstOrDefault(d => d.Id == id);
        if (currentDomain is null)
            yield break;

        while (currentDomain.ParentDomain is not null)
        {
            yield return currentDomain.ParentDomain.Name;
            currentDomain = currentDomain.ParentDomain;
        }
    }
}