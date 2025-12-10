using DomainModel;

namespace ServiceLayer;

public interface IEntityService<TId, TItem> where TItem : IEntity<TId>
{
    void Insert(TItem entity);

    void Update(TItem entity);

    void Delete(TItem entity);

    TItem? GetById(TId id);
}