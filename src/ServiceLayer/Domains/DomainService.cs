using DataMapper;
using DomainModel;
using ServiceLayer.CRUD;

namespace ServiceLayer.Domains;

public interface IDomainService
{
    List<Domain> GetAll();

    bool Add(string domainName, string? parentDomainName = null);
}

public class DomainService(IEntityService<IDomainRepository, int, Domain> _entityService) : IDomainService
{
    public bool Add(string domainName, string? parentDomainName = null)
    {
        if (GetByName(domainName) is not null)
            return false;

        Domain? parentDomain = null;
        if (parentDomainName is not null)
        {
            parentDomain = GetByName(parentDomainName);
            if (parentDomain is null)
                return false;
        }

        var newDomain = new Domain()
        {
            Name = domainName,
            ParentDomain = parentDomain
        };
        _entityService.Insert(newDomain);
        return true;
    }

    public List<Domain> GetAll()
    {
        throw new NotImplementedException();
    }

    private Domain? GetByName(string name) =>
        _entityService.Get(
            select: x => x,
            collector: query => query.SingleOrDefault(),
            filter: x => x.Name == name,
            asNoTracking: true);
}