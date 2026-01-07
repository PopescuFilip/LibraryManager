using DomainModel;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataMapper;

public class Repository<T>(LibraryDbContext _context)
    : IRepository<T>
    where T : class, IEntity
{
    public virtual T Insert(T entity)
    {
        var entityEntry = _context.Set<T>().Add(entity);

        _context.SaveChanges();

        return entityEntry.Entity;
    }

    public T Update(T entity)
    {
        var entityEntry = _context.Set<T>().Update(entity);

        _context.SaveChanges();

        return entityEntry.Entity;
    }

    public void Delete(T entity)
    {
        _context.Set<T>().Remove(entity);

        _context.SaveChanges();
    }

    public T? GetById(int id)
    {
        return _context.Set<T>().Find(id);
    }

    public IReadOnlyCollection<T> GetAllById(IReadOnlyCollection<int> ids)
    {
        return [.. _context.Set<T>()
            .Where(x => ids.Contains(x.Id))
            .OrderBy(x => x.Id)];
    }

    public TOutCollected Get<TOut, TOutCollected>(
        Expression<Func<T, TOut>> select,
        Func<IQueryable<TOut>, TOutCollected> collector,
        bool asNoTracking,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
        Expression<Func<T, bool>>? filter = null,
        params Expression<Func<T, object?>>[] includeProperties)
    {
        var query = asNoTracking
            ? _context.Set<T>().AsNoTracking()
            : _context.Set<T>().AsQueryable();
        var nonExecutedQuery = GetQuery(query, orderBy, filter, includeProperties).Select(select);

        return collector(nonExecutedQuery);
    }

    private static IQueryable<T> GetQuery(
        IQueryable<T> query,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
        Expression<Func<T, bool>>? filter = null,
        params Expression<Func<T, object?>>[] includeProperties)
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