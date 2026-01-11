using DomainModel;
using FluentValidation;
using NLog;
using ServiceLayer.CRUD;
using ServiceLayer.Logging;

namespace ServiceLayer.Decorators;

public class EntityServiceLoggingDecorator<T>(
    IEntityService<T> _entityService,
    INLogLoggerFactory loggerFactory) :
    IEntityService<T> where T : IEntity
{
    private readonly ILogger _logger =
        loggerFactory.GetLogger<EntityServiceLoggingDecorator<T>>();

    public void Delete(T entity)
    {
        _logger.Info($"{nameof(Delete)} started");
        _entityService.Delete(entity);
        _logger.Info($"{nameof(Delete)} ended");

    }

    public IReadOnlyCollection<T> GetAllById(IReadOnlyCollection<int> ids)
    {
        _logger.Info($"{nameof(GetAllById)} started");
        var result = _entityService.GetAllById(ids);
        _logger.Info($"{nameof(GetAllById)} ended");
        return result;
    }

    public T? GetById(int id)
    {
        _logger.Info($"{nameof(GetById)} started");
        var result = _entityService.GetById(id);
        _logger.Info($"{nameof(GetById)} ended");
        return result;
    }

    public Result<T> Insert(T entity, IValidator<T> validator)
    {
        _logger.Info($"{nameof(Insert)} started");
        var result = _entityService.Insert(entity, validator);
        _logger.Info($"{nameof(Insert)} ended");
        return result;
    }

    public bool InsertRange(IReadOnlyCollection<T> entities, IValidator<T> validator)
    {
        _logger.Info($"{nameof(InsertRange)} started");
        var result = _entityService.InsertRange(entities, validator);
        _logger.Info($"{nameof(InsertRange)} ended");
        return result;
    }

    public Result<T> Update(T entity, IValidator<T> validator)
    {
        _logger.Info($"{nameof(Update)} started");
        var result = _entityService.Update(entity, validator);
        _logger.Info($"{nameof(Update)} ended");
        return result;
    }
}