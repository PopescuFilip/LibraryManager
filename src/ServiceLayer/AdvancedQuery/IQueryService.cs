using DomainModel;
using System.Linq.Expressions;

namespace ServiceLayer.AdvancedQuery;

public interface IQueryService<TId, TItem> where TItem : IEntity<TId>
{
    IQueryService<TId, TItem> Where(Expression<Func<TItem, bool>> filter);

    IQueryService<TId, TItem> OrderBy(Func<IQueryable<TItem>, IOrderedQueryable<TItem>> orderBy);

    IQueryService<TId, TItem> Include(params Expression<Func<TItem, object>>[] includeProperties);

    ICollectService<TItem> Collect();

    ICollectService<TOut> Select<TOut>(Expression<Func<TItem, TOut>> filter);
}