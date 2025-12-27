using DomainModel;
using FluentValidation;
using System.Linq.Expressions;

namespace ServiceLayer.CRUD;

public interface IEntityService<TId, TItem>
    where TItem : IEntity<TId>
{
    Result<TItem> Insert(TItem entity, IValidator<TItem> validator);

    bool Update(TItem entity, IValidator<TItem> validator);

    void Delete(TItem entity);

    TItem? GetById(TId id);

    TOutCollected Get<TOut, TOutCollected>(
        Expression<Func<TItem, TOut>> select,
        Func<IQueryable<TOut>, TOutCollected> collector,
        Expression<Func<TItem, bool>>? filter = null,
        Func<IQueryable<TItem>, IOrderedQueryable<TItem>>? orderBy = null,
        bool asNoTracking = true,
        params Expression<Func<TItem, object?>>[] includeProperties);
}