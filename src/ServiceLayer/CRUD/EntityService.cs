using DataMapper;
using DomainModel;
using FluentValidation;

namespace ServiceLayer.CRUD;

public class EntityService<T>(IRepository<T> _repository)
    : IEntityService<T>
    where T : IEntity
{
    public Result<T> Insert(T entity, IValidator<T> validator)
    {
        if (!validator.Validate(entity).IsValid)
            return Result.Invalid();

        var insertedEntity = _repository.Insert(entity);
        return Result.Valid(insertedEntity);
    }

    public bool InsertRange(IReadOnlyCollection<T> entities, IValidator<T> validator)
    {
        if (entities.Any(x => !validator.Validate(x).IsValid))
            return false;

        _repository.InsertRange(entities);
        return true;
    }

    public Result<T> Update(T entity, IValidator<T> validator)
    {
        if (!validator.Validate(entity).IsValid)
            return Result.Invalid();

        var updatedEntity = _repository.Update(entity);
        return Result.Valid(updatedEntity);
    }

    public void Delete(T entity) => _repository.Delete(entity);

    public T? GetById(int id) => _repository.GetById(id);

    public IReadOnlyCollection<T> GetAllById(IReadOnlyCollection<int> ids) =>
        _repository.GetAllById(ids);
}