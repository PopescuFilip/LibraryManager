using DataMapper;
using FluentValidation;
using NSubstitute;
using ServiceLayer.CRUD;
using ServiceLayer.UnitTests.TestHelpers;
using System.Diagnostics.CodeAnalysis;

namespace ServiceLayer.UnitTests;

[TestClass]
[ExcludeFromCodeCoverage]
public class EntityServiceTests
{
    private EntityService<FakeEntity> _entityService = default!;
    private IRepository<FakeEntity> _repository = default!;
    private IValidator<FakeEntity> _validator = default!;

    [TestInitialize]
    public void Init()
    {
        _repository = Substitute.For<IRepository<FakeEntity>>();
        _validator = Substitute.For<IValidator<FakeEntity>>();

        _entityService = new EntityService<FakeEntity>(_repository);
    }

    [TestMethod]
    public void Insert_ShouldReturnInvalid_WhenValidationFails()
    {
        var entity = new FakeEntity();
        _validator.Validate(entity).Returns(Validation.InvalidResult);
        _repository.Insert(entity).Returns(entity);

        var result = _entityService.Insert(entity, _validator);

        Assert.IsFalse(result.IsValid);
    }

    [TestMethod]
    public void Insert_ShouldReturnResultWithInsertedEntity_WhenAllValidationsPass()
    {
        var entity = new FakeEntity();
        var insertedEntity = new FakeEntity() { Id = 2 };
        _validator.Validate(entity).Returns(Validation.ValidResult);
        _repository.Insert(entity).Returns(insertedEntity);

        var result = _entityService.Insert(entity, _validator);

        Assert.IsTrue(result.IsValid);
        var actualEntity = result.Get();
        Assert.AreEqual(insertedEntity, actualEntity);
    }

    [TestMethod]
    public void Insert_ShouldReturnValid_WhenAllValidationsPass()
    {
        var entity = new FakeEntity();
        _validator.Validate(entity).Returns(Validation.ValidResult);
        _repository.Insert(entity).Returns(entity);

        var result = _entityService.Insert(entity, _validator);

        Assert.IsTrue(result.IsValid);
    }

    [TestMethod]
    public void InsertRange_ShouldReturnFalse_WhenAnyValidationFails()
    {
        var entities = Enumerable.Range(0, 5)
            .Select(id => new FakeEntity() { Id = id })
            .ToList();
        var invalidEntity = entities.Skip(2).First();
        _validator.Validate(Arg.Any<FakeEntity>())
            .Returns(Validation.ValidResult);
        _validator.Validate(Arg.Is<FakeEntity>(x => x == invalidEntity))
            .Returns(Validation.InvalidResult);

        var success = _entityService.InsertRange(entities, _validator);

        Assert.IsFalse(success);
        _repository.DidNotReceiveWithAnyArgs().InsertRange(entities);
    }

    [TestMethod]
    public void InsertRange_ShouldReturnTrue_WhenAllValidationsPass()
    {
        var entities = Enumerable.Range(0, 5)
            .Select(id => new FakeEntity() { Id = id })
            .ToList();
        _validator.Validate(Arg.Is<FakeEntity>(x => entities.Contains(x)))
            .Returns(Validation.ValidResult);

        var success = _entityService.InsertRange(entities, _validator);

        Assert.IsTrue(success);
        _repository.Received(1).InsertRange(entities);
    }

    [TestMethod]
    public void Update_ShouldReturnInvalid_WhenValidationFails()
    {
        var entity = new FakeEntity();
        _validator.Validate(entity).Returns(Validation.InvalidResult);
        _repository.Update(entity).Returns(entity);

        var result = _entityService.Update(entity, _validator);

        Assert.IsFalse(result.IsValid);
    }

    [TestMethod]
    public void Update_ShouldReturnResultWithUpdatedEntity_WhenAllValidationsPass()
    {
        var entity = new FakeEntity();
        var updatedEntity = new FakeEntity() { Id = 2 };
        _validator.Validate(entity).Returns(Validation.ValidResult);
        _repository.Update(entity).Returns(updatedEntity);

        var result = _entityService.Update(entity, _validator);

        Assert.IsTrue(result.IsValid);
        var actualEntity = result.Get();
        Assert.AreEqual(updatedEntity, actualEntity);
    }

    [TestMethod]
    public void Update_ShouldReturnValid_WhenAllValidationsPass()
    {
        var entity = new FakeEntity();
        _validator.Validate(entity).Returns(Validation.ValidResult);
        _repository.Update(entity).Returns(entity);

        var result = _entityService.Update(entity, _validator);

        Assert.IsTrue(result.IsValid);
    }

    [TestMethod]
    public void Delete_ShouldCallRepositoryDelete()
    {
        var entity = new FakeEntity();

        _entityService.Delete(entity);

        _repository.Received(1).Delete(entity);
    }

    [TestMethod]
    public void GetById_ShouldReturnNull_WhenEntityIsNotFound()
    {
        var id = 345;
        _repository.GetById(id).Returns((FakeEntity?)null);

        var foundEntity = _entityService.GetById(id);

        Assert.IsNull(foundEntity);
    }

    [TestMethod]
    public void GetById_ShouldReturnEntity_WhenItExists()
    {
        var id = 345;
        var entity = new FakeEntity() { Id = id };
        _repository.GetById(id).Returns(entity);

        var foundEntity = _entityService.GetById(id);

        Assert.IsNotNull(foundEntity);
        Assert.AreEqual(entity, foundEntity);
    }

    [TestMethod]
    public void GetAllById_ShouldReturnFoundEntities_WhenAllValidationsPass()
    {
        var ids = Enumerable.Range(1, 5).ToList();
        var entities = ids
            .Select(id => new FakeEntity() { Id = id })
            .ToList();
        _repository.GetAllById(ids).Returns(entities);

        var actualEntities = _entityService.GetAllById(ids);

        Assert.AreEqual(entities, actualEntities);
    }
}