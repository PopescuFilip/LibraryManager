using DomainModel;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataMapper;

public class Repository<T>(IDbContextFactory<LibraryDbContext> _dbContextFactory)
    : IRepository<int, T> where T : class, IEntity<int>
{
    public void Delete(T entity)
    {
        using var context = _dbContextFactory.CreateDbContext();
        context.Set<T>().Remove(entity);

        context.SaveChanges();
    }

    public List<T> Get(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        params Expression<Func<T, object>>[] includeProperties)
    {
        using var context = _dbContextFactory.CreateDbContext();

        var query = context.Set<T>().AsQueryable();

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

        return query.ToList();
    }

    public T? GetById(int id)
    {
        using var context = _dbContextFactory.CreateDbContext();
        return context.Set<T>().Find(id);
    }

    public void Insert(T entity)
    {
        using var context = _dbContextFactory.CreateDbContext();
        context.Set<T>().Add(entity);

        context.SaveChanges();
    }

    public void Update(T entity)
    {
        using var context = _dbContextFactory.CreateDbContext();
        context.Set<T>().Update(entity);

        context.SaveChanges();
    }
}