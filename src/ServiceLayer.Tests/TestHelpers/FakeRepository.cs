using DataMapper;
using DomainModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace ServiceLayer.UnitTests.TestHelpers;

[ExcludeFromCodeCoverage]
public class FakeRepository<T> : IRepository<T> where T : IEntity
{
    private List<T> _values = [];

    public void SetSourceValues(List<T> values)
    {
        _values = values;
    }

    public void Delete(T entity)
    {
        throw new NotImplementedException();
    }

    public TOutCollected Get<TOut, TOutCollected>(
        Expression<Func<T, TOut>> select,
        Func<IQueryable<TOut>, TOutCollected> collector,
        bool asNoTracking,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
        Expression<Func<T, bool>>? filter = null,
        params Expression<Func<T, object?>>[] includeProperties)
    {
        var query = _values.AsQueryable();
        if (filter is not null)
        {
            query = query.Where(filter);
        }

        return collector(orderBy(query).Select(select));
    }

    public IReadOnlyCollection<T> GetAllById(IReadOnlyCollection<int> ids)
    {
        throw new NotImplementedException();
    }

    public T? GetById(int id)
    {
        throw new NotImplementedException();
    }

    public T Insert(T entity)
    {
        throw new NotImplementedException();
    }

    public T Update(T entity)
    {
        throw new NotImplementedException();
    }
}