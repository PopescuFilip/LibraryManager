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
    IEntityService<int, Domain> _entityService,
    IDomainQueryService domainQueryService,
    IValidator<Domain> validator)
    : IDomainService
{
    public bool Add(string domainName, string? parentDomainName = null)
    {
        if (domainQueryService.GetIdByName(domainName) is not null)
            return false;

        int? parentDomainId = null;
        if (parentDomainName is not null)
        {
            parentDomainId = domainQueryService.GetIdByName(parentDomainName);
            if (parentDomainId is null)
                return false;
        }

        var newDomain = new Domain(domainName, parentDomainId);
        return _entityService.Insert(newDomain, validator).IsValid;
    }
}