using DomainModel;
using System.Linq.Expressions;

namespace DataMapper;

public interface IRepository<TId, TItem> where TItem : IEntity<TId>
{
    void Insert(TItem entity);

    void Update(TItem entity);

    void Delete(TItem entity);

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

    TOutCollected GetCollected<TOut, TOutCollected>(
        Expression<Func<TItem, TOut>> select,
        Func<IQueryable<TOut>, TOutCollected> collector,
        Expression<Func<TItem, bool>>? filter = null,
        Func<IQueryable<TItem>, IOrderedQueryable<TItem>>? orderBy = null,
        params Expression<Func<TItem, object>>[] includeProperties);

    TOutCollected Get<TOutCollected>(
        Func<IQueryable<TItem>, TOutCollected> collector,
        Expression<Func<TItem, bool>>? filter = null,
        Func<IQueryable<TItem>, IOrderedQueryable<TItem>>? orderBy = null,
        params Expression<Func<TItem, object>>[] includeProperties);
}