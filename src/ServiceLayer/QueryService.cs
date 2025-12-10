using DataMapper;
using DomainModel;
using System.Linq.Expressions;

namespace ServiceLayer;

public class QueryService<TId, TItem>(IRepository<TId, TItem> _repository)
    : IQueryService<TId, TItem> where TItem : IEntity<TId>
{
    public bool Delete(TItem entity)
    {
        _repository.Delete(entity);
        return true;
    }

    public List<TOut> Get<TOut>(
        Expression<Func<TItem, TOut>> select,
        Expression<Func<TItem, bool>>? filter = null,
        Func<IQueryable<TItem>, IOrderedQueryable<TItem>>? orderBy = null,
        params Expression<Func<TItem, object>>[] includeProperties) =>
        _repository.Get(select, filter, orderBy, includeProperties);

    public List<TItem> Get(
        Expression<Func<TItem, bool>>? filter = null,
        Func<IQueryable<TItem>, IOrderedQueryable<TItem>>? orderBy = null,
        params Expression<Func<TItem, object>>[] includeProperties) =>
        _repository.Get(filter, orderBy, includeProperties);

    public TItem? GetById(TId id) => _repository.GetById(id);

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