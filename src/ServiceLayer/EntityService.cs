using DataMapper;
using DomainModel;

namespace ServiceLayer;

public class EntityService<TId, TItem>(IRepository<TId, TItem> _repository)
    : IEntityService<TId, TItem> where TItem : IEntity<TId>
{
    public void Delete(TItem entity) => _repository.Delete(entity);

    public IQueryService<TId, TItem> Get()
    {
        throw new NotImplementedException();
    }

    public TItem? GetById(TId id) => _repository.GetById(id);

    public void Insert(TItem entity) => _repository.Insert(entity);

    public void Update(TItem entity) => _repository.Update(entity);
}