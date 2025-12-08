using DomainModel;
using System.Linq.Expressions;

namespace DataMapper;

public interface IRepository<TId, TItem> where TItem : IEntity<TId>
{
    void Insert(TItem entity);

    void Update(TItem entity);

    void Delete(TItem entity);

    TItem? GetById(TId id);

    List<TItem> Get(
        Expression<Func<TItem, bool>>? filter = null,
        Expression<Func<TItem, object>>? orderBy = null,
        List<Func<TItem, object>>? includeProperties = null);
}