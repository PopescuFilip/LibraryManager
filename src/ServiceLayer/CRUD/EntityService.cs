using DataMapper;
using DomainModel;
using FluentValidation;

namespace ServiceLayer.CRUD;

public class EntityService<TId, TItem>(IRepository<TId, TItem> _repository)
    : IEntityService<TId, TItem>
    where TItem : IEntity<TId>
{
    public Result<TItem> Insert(TItem entity, IValidator<TItem> validator)
    {
        if (!validator.Validate(entity).IsValid)
            return Result.Invalid();

        var insertedEntity = _repository.Insert(entity);
        return Result.Valid(insertedEntity);
    }

    public bool Update(TItem entity, IValidator<TItem> validator)
    {
        if (!validator.Validate(entity).IsValid)
            return false;

        _repository.Update(entity);
        return true;
    }

    public void Delete(TItem entity) => _repository.Delete(entity);

    public TItem? GetById(TId id) => _repository.GetById(id);

    public IReadOnlyCollection<TItem> GetAllById(IReadOnlyCollection<TId> ids) =>
        _repository.GetAllById(ids);
}