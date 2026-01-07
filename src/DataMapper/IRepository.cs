using DomainModel;
using System.Linq.Expressions;

namespace DataMapper;

public interface IRepository<T> where T : IEntity
{
    T Insert(T entity);

    T Update(T entity);

    void Delete(T entity);

    T? GetById(int id);

    IReadOnlyCollection<T> GetAllById(IReadOnlyCollection<int> ids);

    TOutCollected Get<TOut, TOutCollected>(
        Expression<Func<T, TOut>> select,
        Func<IQueryable<TOut>, TOutCollected> collector,
        bool asNoTracking,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
        Expression<Func<T, bool>>? filter = null,
        params Expression<Func<T, object?>>[] includeProperties);
}