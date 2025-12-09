using DataMapper;
using DomainModel;
using System.Linq.Expressions;

namespace ServiceLayer;

public class EntityService<T>(IRepository<int, T> _repository)
    : IEntityService<int, T> where T : IEntity<int>
{
    public bool Delete(T entity)
    {
        _repository.Delete(entity);
        return true;
    }

    public List<TOut> Get<TOut>(
        Expression<Func<T, TOut>> select,
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        params Expression<Func<T, object>>[] includeProperties) =>
        _repository.Get(select, filter, orderBy, includeProperties);

    public List<T> Get(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        params Expression<Func<T, object>>[] includeProperties) =>
        _repository.Get(filter, orderBy, includeProperties);

    public T? GetById(int id) => _repository.GetById(id);

    public bool Insert(T entity)
    {
        _repository.Insert(entity);
        return true;
    }

    public bool Update(T entity)
    {
        _repository.Update(entity);
        return true;
    }
}