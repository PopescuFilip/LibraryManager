using DomainModel;
using System.Linq.Expressions;

namespace ServiceLayer;

public interface IQueryService<TId, TItem> where TItem : IEntity<TId>
{
    TItem? GetById(TId id);

    List<TOut> Get<TOut>(
        Expression<Func<TItem, TOut>> select,
        Expression<Func<TItem, bool>>? filter = null,
        Func<IQueryable<TItem>, IOrderedQueryable<TItem>>? orderBy = null,
        params Expression<Func<TItem, object>>[] includeProperties);

    List<TItem> Get(
        Expression<Func<TItem, bool>>? filter = null,
        Func<IQueryable<TItem>, IOrderedQueryable<TItem>>? orderBy = null,
        params Expression<Func<TItem, object>>[] includeProperties);
}