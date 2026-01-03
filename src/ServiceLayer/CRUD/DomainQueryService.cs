using DataMapper;
using DataMapper.QueryHelpers;
using DomainModel;

namespace ServiceLayer.CRUD;

public interface IDomainQueryService
{
    int? GetIdByName(string name);

    IEnumerable<string> GetImplicitDomainNames(int id);
}

public class DomainQueryService(IRepository<int, Domain> _repository)
    : IDomainQueryService
{
    public int? GetIdByName(string name)
    {
        var foundIds = _repository.Get(
            select: d => d.Id,
            collector: q => q.Take(2).ToList(),
            filter: d => d.Name == name,
            orderBy: Order.ById<Domain>(),
            asNoTracking: true
            );

        if (foundIds.Count == 2)
            throw new InvalidOperationException($"Found more than one Domain with name: {name}");

        return foundIds.Count == 0 ? null : foundIds.First();
    }

    public IEnumerable<string> GetImplicitDomainNames(int id)
    {
        var allDomains = _repository.Get(
            select: d => d,
            collector: q => q.ToList(),
            asNoTracking: false,
            orderBy: Order.ById<Domain>(),
            includeProperties: d => d.ParentDomain
            );

        var currentDomain = allDomains.FirstOrDefault(d => d.Id == id);

        while (currentDomain is not null)
        {
            yield return currentDomain.Name;
            currentDomain = currentDomain.ParentDomain;
        }
    }
}