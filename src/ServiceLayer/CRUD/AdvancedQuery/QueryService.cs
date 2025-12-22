using DomainModel;
using System.Collections.Immutable;
using System.Linq.Expressions;

namespace ServiceLayer.CRUD.AdvancedQuery;

public record Query<TId, TItem>(
    Expression<Func<TItem, bool>>? Filter,
    Func<IQueryable<TItem>, IOrderedQueryable<TItem>>? OrderBy,
    ImmutableList<Expression<Func<TItem, object>>> IncludeProperties)
    where TItem : IEntity<TId>
{
    public static Query<TId, TItem> Empty => new(null, null, []);

    public Query<TId, TItem> Where(Expression<Func<TItem, bool>> where) =>
        this with { Filter = where };

    public Query<TId, TItem> Include(Expression<Func<TItem, object>> include) =>
        this with { IncludeProperties = IncludeProperties.Add(include) };
}

public class QueryService<TId, TItem>()
    : IQueryService<TId, TItem> where TItem : IEntity<TId>
{
    private readonly Expression<Func<TItem, bool>>? _filter;
    private Func<IQueryable<TItem>, IOrderedQueryable<TItem>> _orderBy =
        (query) => query.OrderBy(x => x.Id);
    private Expression<Func<TItem, object>>[] includeProperties;

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