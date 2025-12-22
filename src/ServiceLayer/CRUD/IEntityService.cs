using DataMapper;
using DomainModel;
using System.Linq.Expressions;

namespace ServiceLayer.CRUD;

public interface IEntityService<R, TId, TItem>
    where R : IRepository<TId, TItem>
    where TItem : IEntity<TId>
{
    void Insert(TItem entity);

    void Update(TItem entity);

    void Delete(TItem entity);

    TItem? GetById(TId id);

    TOutCollected Get<TOut, TOutCollected>(
        Expression<Func<TItem, TOut>> select,
        Func<IQueryable<TOut>, TOutCollected> collector,
        Expression<Func<TItem, bool>>? filter = null,
        Func<IQueryable<TItem>, IOrderedQueryable<TItem>>? orderBy = null,
        bool asNoTracking = false,
        params Expression<Func<TItem, object?>>[] includeProperties);
}