using DataMapper;
using DomainModel;
using System.Linq.Expressions;

namespace ServiceLayer;

public class EntityService<TId, TItem>(IRepository<TId, TItem> _repository)
    : IEntityService<TId, TItem> where TItem : IEntity<TId>
{
    public void Delete(TItem entity) => _repository.Delete(entity);

    public TItem? GetById(TId id) => _repository.GetById(id);

    public void Insert(TItem entity) => _repository.Insert(entity);

    public void Update(TItem entity) => _repository.Update(entity);

    public TOutCollected Get<TOut, TOutCollected>(
        Expression<Func<TItem, TOut>> select,
        Func<IQueryable<TOut>, TOutCollected> collector,
        Expression<Func<TItem, bool>>? filter = null,
        Func<IQueryable<TItem>, IOrderedQueryable<TItem>>? orderBy = null,
        bool asNoTracking = false,
        params Expression<Func<TItem, object>>[] includeProperties)
    {
        orderBy ??= query => query.OrderBy(x => x.Id);
        return _repository.Get(select, collector, filter, orderBy, asNoTracking, includeProperties);
    }
}