using DomainModel;
using NLog;
using NSubstitute;
using ServiceLayer.CRUD;
using ServiceLayer.Decorators;
using ServiceLayer.Logging;
using ServiceLayer.UnitTests.TestHelpers;
using System.Diagnostics.CodeAnalysis;

namespace ServiceLayer.UnitTests;

[TestClass]
[ExcludeFromCodeCoverage]
public class EntityServiceLoggingDecoratorTests
{
    private EntityServiceLoggingDecorator<FakeEntity> _entityServiceDecorator = default!;
    private IEntityService<FakeEntity> _entityService = default!;
    private INLogLoggerFactory _loggerFactory = default!;
    private ILogger _logger = default!;

    [TestInitialize]
    public void Init()
    {
        _entityService = Substitute.For<IEntityService<FakeEntity>>();
        _loggerFactory = Substitute.For<INLogLoggerFactory>();
        _logger = Substitute.For<ILogger>();

        _loggerFactory.GetLogger<EntityServiceLoggingDecorator<FakeEntity>>()
            .Returns(_logger);

        _entityServiceDecorator = new EntityServiceLoggingDecorator<FakeEntity>(
            _entityService,
            _loggerFactory);
    }

    [TestMethod]
    public void Delete_ShouldCallDecoratedMethod()
    {
        var entity = new FakeEntity();

        _entityServiceDecorator.Delete(entity);

        _entityService.Received(1).Delete(entity);
    }

    [TestMethod]
    public void GetAllById_ShouldCallDecoratedMethod()
    {
        var ids = new List<int>() { 1, 2, 54, 65 };
        var entities = ids.Select(id => new FakeEntity { Id = id }).ToList();
        _entityService.GetAllById(ids).Returns(entities);

        var actualEntities = _entityServiceDecorator.GetAllById(ids);

        Assert.AreEqual(entities, actualEntities);
        _entityService.Received(1).GetAllById(ids);
    }

    [TestMethod]
    public void GetById_ShouldCallDecoratedMethod()
    {
        var id = 123;
        var entity = new FakeEntity() { Id = id };
        _entityService.GetById(id).Returns(entity);

        var actualEntity = _entityServiceDecorator.GetById(id);

        Assert.AreEqual(entity, actualEntity);
        _entityService.Received(1).GetById(id);
    }

    [TestMethod]
    public void GetById_ShouldReturnNullWhenNullIsReturned()
    {
        var id = 123;
        _entityService.GetById(id).Returns((FakeEntity?)null);

        var actualEntity = _entityServiceDecorator.GetById(id);

        Assert.IsNull(actualEntity);
        _entityService.Received(1).GetById(id);
    }

    [TestMethod]
    public void Insert_ShouldCallDecoratedMethod()
    {
        var id = 123;
        var validator = EmptyValidator.Create<FakeEntity>();
        var entity = new FakeEntity() { Id = id };
        var result = Result.Valid(entity);
        _entityService.Insert(entity, validator).Returns(result);

        var actualResult = _entityServiceDecorator.Insert(entity, validator);

        Assert.AreEqual(result, actualResult);
        _entityService.Received(1).Insert(entity, validator);
    }

    [TestMethod]
    public void InsertRange_ShouldCallDecoratedMethod()
    {
        var ids = new List<int>() { 1, 2, 54, 65 };
        var entities = ids.Select(id => new FakeEntity { Id = id }).ToList();
        var validator = EmptyValidator.Create<FakeEntity>();
        var success = true;
        _entityService.InsertRange(entities, validator).Returns(success);

        var actualResult = _entityServiceDecorator.InsertRange(entities, validator);

        Assert.AreEqual(success, actualResult);
        _entityService.Received(1).InsertRange(entities, validator);
    }

    [TestMethod]
    public void Update_ShouldCallDecoratedMethod()
    {
        var id = 123;
        var validator = EmptyValidator.Create<FakeEntity>();
        var entity = new FakeEntity() { Id = id };
        var result = Result.Valid(entity);
        _entityService.Update(entity, validator).Returns(result);

        var actualResult = _entityServiceDecorator.Update(entity, validator);

        Assert.AreEqual(result, actualResult);
        _entityService.Received(1).Update(entity, validator);
    }
}