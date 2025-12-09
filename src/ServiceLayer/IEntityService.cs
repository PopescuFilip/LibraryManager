using DomainModel;
using System.Linq.Expressions;

namespace ServiceLayer;

public interface IEntityService<TId, TItem> where TItem : IEntity<TId>
{
    bool Insert(TItem entity);

    bool Update(TItem entity);

    bool Delete(TItem entity);

    TItem? GetById(TId id);

    public List<TOut> Get<TOut>(
        Expression<Func<TItem, TOut>> select,
        Expression<Func<TItem, bool>>? filter = null,
        Func<IQueryable<TItem>, IOrderedQueryable<TItem>>? orderBy = null,
        params Expression<Func<TItem, object>>[] includeProperties);

    List<TItem> Get(
        Expression<Func<TItem, bool>>? filter = null,
        Func<IQueryable<TItem>, IOrderedQueryable<TItem>>? orderBy = null,
        params Expression<Func<TItem, object>>[] includeProperties);
}