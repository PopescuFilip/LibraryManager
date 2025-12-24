using DataMapper;
using DomainModel;
using FluentValidation;
using ServiceLayer.CRUD;

namespace ServiceLayer.Domains;

public interface IDomainService
{
    bool Add(string domainName, string? parentDomainName = null);
}

public class DomainService(
    IEntityService<IDomainRepository, int, Domain> _entityService,
    IValidator<Domain> validator)
    : IDomainService
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

        var newDomain = Domain.CreateNew(domainName, parentDomain);
        return _entityService.Insert(newDomain, validator);
    }

    private Domain? GetByName(string name) =>
        _entityService.Get(
            select: x => x,
            collector: query => query.SingleOrDefault(),
            filter: x => x.Name == name,
            asNoTracking: true);
}