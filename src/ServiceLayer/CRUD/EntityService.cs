using DataMapper;
using DomainModel;
using FluentValidation;
using System.Linq.Expressions;

namespace ServiceLayer.CRUD;

public class EntityService<R, TId, TItem>(R _repository)
    : IEntityService<R, TId, TItem>
    where R : IRepository<TId, TItem>
    where TItem : IEntity<TId>
{
    public void Delete(TItem entity) => _repository.Delete(entity);

    public TItem? GetById(TId id) => _repository.GetById(id);

    public bool Insert(TItem entity, IValidator<TItem> validator)
    {
        if (!validator.Validate(entity).IsValid)
            return false;

        _repository.Insert(entity);
        return true;
    }

    public bool Update(TItem entity, IValidator<TItem> validator)
    {
        if (!validator.Validate(entity).IsValid)
            return false;

        _repository.Update(entity);
        return true;
    }

    public TOutCollected Get<TOut, TOutCollected>(
        Expression<Func<TItem, TOut>> select,
        Func<IQueryable<TOut>, TOutCollected> collector,
        Expression<Func<TItem, bool>>? filter = null,
        Func<IQueryable<TItem>, IOrderedQueryable<TItem>>? orderBy = null,
        bool asNoTracking = true,
        params Expression<Func<TItem, object?>>[] includeProperties)
    {
        orderBy ??= query => query.OrderBy(x => x.Id);
        return _repository.Get(select, collector, filter, orderBy, asNoTracking, includeProperties);
    }
}