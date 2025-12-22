using DataMapper;
using DomainModel;
using FluentValidation;
using ServiceLayer.CRUD;

namespace ServiceLayer.Domains;

public interface IDomainEntityService : IEntityService<IDomainRepository, int, Domain>
{
    Domain? GetByName(string name);
}

public class DomainEntityService(
    IDomainRepository repository,
    IValidator<Domain> _validator)
    : EntityService<IDomainRepository, int, Domain>(repository), IDomainEntityService
{
    public Domain? GetByName(string name)
    {
        repository.Get(
            select: x => x,
            collector: query => query.SingleOrDefault(),
            filter: x => x.Name == name,
            asNoTracking: true);
    }
}