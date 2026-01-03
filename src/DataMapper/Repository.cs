using DomainModel;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataMapper;

public class Repository<TId, TItem>(LibraryDbContext _context)
    : IRepository<TId, TItem>
    where TItem : class, IEntity<TId>
{
    public virtual TItem Insert(TItem entity)
    {
        var entityEntry = _context.Set<TItem>().Add(entity);

        _context.SaveChanges();

        return entityEntry.Entity;
    }

    public void Update(TItem entity)
    {
        _context.Set<TItem>().Update(entity);

        _context.SaveChanges();
    }

    public void Delete(TItem entity)
    {
        _context.Set<TItem>().Remove(entity);

        _context.SaveChanges();
    }

    public TItem? GetById(TId id)
    {
        return _context.Set<TItem>().Find(id);
    }

    public IReadOnlyCollection<TItem> GetAllById(IReadOnlyCollection<TId> ids)
    {
        return [.. _context.Set<TItem>()
            .Where(x => ids.Contains(x.Id))
            .OrderBy(x => x.Id)];
    }

    public TOutCollected Get<TOut, TOutCollected>(
        Expression<Func<TItem, TOut>> select,
        Func<IQueryable<TOut>, TOutCollected> collector,
        bool asNoTracking,
        Func<IQueryable<TItem>, IOrderedQueryable<TItem>> orderBy,
        Expression<Func<TItem, bool>>? filter = null,
        params Expression<Func<TItem, object?>>[] includeProperties)
    {
        var query = asNoTracking
            ? _context.Set<TItem>().AsNoTracking()
            : _context.Set<TItem>().AsQueryable();
        var nonExecutedQuery = GetQuery(query, orderBy, filter, includeProperties).Select(select);

        return collector(nonExecutedQuery);
    }

    private static IQueryable<TItem> GetQuery(
        IQueryable<TItem> query,
        Func<IQueryable<TItem>, IOrderedQueryable<TItem>> orderBy,
        Expression<Func<TItem, bool>>? filter = null,
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

        return orderBy(query);
    }
}