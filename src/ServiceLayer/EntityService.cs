using DataMapper;
using DomainModel;

namespace ServiceLayer;

public class EntityService<TId, TItem>(IRepository<TId, TItem> _repository)
    : IEntityService<TId, TItem> where TItem : IEntity<TId>
{
    public bool Delete(TItem entity)
    {
        _repository.Delete(entity);
        return true;
    }

    public bool Insert(TItem entity)
    {
        _repository.Insert(entity);
        return true;
    }

    public bool Update(TItem entity)
    {
        _repository.Update(entity);
        return true;
    }
}