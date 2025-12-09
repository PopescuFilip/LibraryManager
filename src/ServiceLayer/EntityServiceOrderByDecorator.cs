using DomainModel;
using System.Linq.Expressions;

namespace ServiceLayer;

public class EntityServiceOrderByDecorator<TId, TItem>(
    IEntityService<TId, TItem> _entityService)
    : IEntityService<TId, TItem> where TItem : IEntity<TId>
{
    public bool Delete(TItem entity) => _entityService.Delete(entity);

    public List<TOut> Get<TOut>(
        Expression<Func<TItem, TOut>> select,
        Expression<Func<TItem, bool>>? filter = null,
        Func<IQueryable<TItem>, IOrderedQueryable<TItem>>? orderBy = null,
        params Expression<Func<TItem, object>>[] includeProperties)
    {
        orderBy ??= (query) => query.OrderBy(x => x.Id);
        return _entityService.Get(select, filter, orderBy, includeProperties);
    }

    public List<TItem> Get(
        Expression<Func<TItem, bool>>? filter = null,
        Func<IQueryable<TItem>, IOrderedQueryable<TItem>>? orderBy = null,
        params Expression<Func<TItem, object>>[] includeProperties)
    {
        orderBy ??= (query) => query.OrderBy(x => x.Id);
        return _entityService.Get(filter, orderBy, includeProperties);
    }

    public TItem? GetById(TId id) => _entityService.GetById(id);

    public bool Insert(TItem entity) => _entityService.Insert(entity);

    public bool Update(TItem entity) => _entityService.Update(entity);
}