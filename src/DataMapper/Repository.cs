using DomainModel;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataMapper;

public class Repository<TId, TItem>(IDbContextFactory<LibraryDbContext> _dbContextFactory)
    : IRepository<TId, TItem>
    where TItem : class, IEntity<TId>
{
    public virtual TItem Insert(TItem entity)
    {
        using var context = _dbContextFactory.CreateDbContext();
        var entityEntry = context.Set<TItem>().Add(entity);

        context.SaveChanges();

        return entityEntry.Entity;
    }

    public void Update(TItem entity)
    {
        using var context = _dbContextFactory.CreateDbContext();
        context.Set<TItem>().Update(entity);

        context.SaveChanges();
    }

    public void Delete(TItem entity)
    {
        using var context = _dbContextFactory.CreateDbContext();
        context.Set<TItem>().Remove(entity);

        context.SaveChanges();
    }

    public TItem? GetById(TId id)
    {
        using var context = _dbContextFactory.CreateDbContext();
        return context.Set<TItem>().Find(id);
    }

    public IReadOnlyCollection<TItem> GetAllById(IReadOnlyCollection<TId> ids)
    {
        using var context = _dbContextFactory.CreateDbContext();
        return [.. context.Set<TItem>()
            .AsNoTracking()
            .Where(x => ids.Contains(x.Id))
            .OrderBy(x => x.Id)];
    }

    public TOutCollected Get<TOut, TOutCollected>(
        Expression<Func<TItem, TOut>> select,
        Func<IQueryable<TOut>, TOutCollected> collector,
        Expression<Func<TItem, bool>>? filter = null,
        Func<IQueryable<TItem>, IOrderedQueryable<TItem>>? orderBy = null,
        bool asNoTracking = false,
        params Expression<Func<TItem, object?>>[] includeProperties)
    {
        using var context = _dbContextFactory.CreateDbContext();
        var query = asNoTracking
            ? context.Set<TItem>().AsNoTracking()
            : context.Set<TItem>().AsQueryable();
        var nonExecutedQuery = GetQuery(query, filter, orderBy, includeProperties).Select(select);

        return collector(nonExecutedQuery);
    }

    private static IQueryable<TItem> GetQuery(
        IQueryable<TItem> query,
        Expression<Func<TItem, bool>>? filter = null,
        Func<IQueryable<TItem>, IOrderedQueryable<TItem>>? orderBy = null,
        params Expression<Func<TItem, object?>>[] includeProperties)
    {
        if (includeProperties is not null)
        {
            foreach (var property in includeProperties)
            {
                query = query.Include(property);
            }
        }

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        if (orderBy is not null)
        {
            query = orderBy(query);
        }

        return query;
    }
}