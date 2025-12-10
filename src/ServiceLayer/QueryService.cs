using DataMapper;
using DomainModel;
using System.Linq.Expressions;

namespace ServiceLayer;

public class QueryService<TId, TItem>(IRepository<TId, TItem> _repository)
    : IQueryService<TId, TItem> where TItem : IEntity<TId>
{
    public ICollectService<TItem> Collect()
    {
        throw new NotImplementedException();
    }

    public IQueryService<TId, TItem> Include(params Expression<Func<TItem, object>>[] includeProperties)
    {
        throw new NotImplementedException();
    }

    public IQueryService<TId, TItem> OrderBy(Func<IQueryable<TItem>, IOrderedQueryable<TItem>> orderBy)
    {
        throw new NotImplementedException();
    }

    public ICollectService<TOut> Select<TOut>(Expression<Func<TItem, TOut>> filter)
    {
        throw new NotImplementedException();
    }

    public IQueryService<TId, TItem> Where(Expression<Func<TItem, bool>> filter)
    {
        throw new NotImplementedException();
    }
}