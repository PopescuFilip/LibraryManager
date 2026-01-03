using DomainModel;
using System.Linq.Expressions;

namespace DataMapper;

public interface IRepository<TId, TItem> where TItem : IEntity<TId>
{
    TItem Insert(TItem entity);

    void Update(TItem entity);

    void Delete(TItem entity);

    TItem? GetById(TId id);

    IReadOnlyCollection<TItem> GetAllById(IReadOnlyCollection<TId> ids);

    TOutCollected Get<TOut, TOutCollected>(
        Expression<Func<TItem, TOut>> select,
        Func<IQueryable<TOut>, TOutCollected> collector,
        bool asNoTracking,
        Func<IQueryable<TItem>, IOrderedQueryable<TItem>> orderBy,
        Expression<Func<TItem, bool>>? filter = null,
        params Expression<Func<TItem, object?>>[] includeProperties);
}