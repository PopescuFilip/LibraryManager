using DomainModel;
using FluentValidation;

namespace ServiceLayer.CRUD;

public interface IEntityService<T> where T : IEntity
{
    Result<T> Insert(T entity, IValidator<T> validator);

    bool Update(T entity, IValidator<T> validator);

    void Delete(T entity);

    T? GetById(int id);

    IReadOnlyCollection<T> GetAllById(IReadOnlyCollection<int> ids);
}