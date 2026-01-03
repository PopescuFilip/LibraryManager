using DomainModel;
using FluentValidation;

namespace ServiceLayer.CRUD;

public interface IEntityService<TId, TItem>
    where TItem : IEntity<TId>
{
    Result<TItem> Insert(TItem entity, IValidator<TItem> validator);

    bool Update(TItem entity, IValidator<TItem> validator);

    void Delete(TItem entity);

    TItem? GetById(TId id);

    IReadOnlyCollection<TItem> GetAllById(IReadOnlyCollection<TId> ids);
}