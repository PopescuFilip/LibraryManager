using DomainModel;
using DomainModel.Restrictions;
using FluentValidation;
using NSubstitute;
using ServiceLayer.Borrowing;
using ServiceLayer.CRUD;
using ServiceLayer.Restriction;
using ServiceLayer.UnitTests.TestHelpers;
using System.Diagnostics.CodeAnalysis;

namespace ServiceLayer.UnitTests;

[TestClass]
[ExcludeFromCodeCoverage]
public class BorrowServiceTests
{
    private BorrowService _borrowService = default!;
    private IEntityService<BorrowRecord> _entityService = default!;
    private IEntityService<Client> _clientEntityService = default!;
    private IEntityService<Employee> _employeeEntityService = default!;
    private IValidator<IdCollection> _idCollectionValidator = default!;
    private IRestrictionsService _restrictionsService = default!;
    private IEmployeeRestrictionsProvider _employeeRestrictionsProvider = default!;
    private IBorrowRecordQueryService _borrowRecordQueryService = default!;

    [TestInitialize]
    public void Init()
    {
        _entityService = Substitute.For<IEntityService<BorrowRecord>>();
        _clientEntityService = Substitute.For<IEntityService<Client>>();
        _employeeEntityService = Substitute.For<IEntityService<Employee>>();
        _idCollectionValidator = Substitute.For<IValidator<IdCollection>>();
        _restrictionsService = Substitute.For<IRestrictionsService>();
        _employeeRestrictionsProvider = Substitute.For<IEmployeeRestrictionsProvider>();
        _borrowRecordQueryService = Substitute.For<IBorrowRecordQueryService>();

        _borrowService = new BorrowService(
            _entityService,
            _clientEntityService,
            _employeeEntityService,
            _idCollectionValidator,
            _restrictionsService,
            _employeeRestrictionsProvider,
            _borrowRecordQueryService
            );
    }

    [TestMethod]
    public void Borrow_ShouldReturnFalse_WhenInsertFails()
    {
        var borrowerId = 1;
        var lenderId = 4;
        var ids = new List<int>() { 1, 4, 5, 7, 9 }.ToIdCollection();
        var borrowerAccountId = 21;
        var borrower = new Client(borrowerAccountId) { Id = borrowerId };
        var lenderAccountId = 32;
        var lender = new Employee(lenderAccountId) { Id = lenderId };
        var clientRestrictions = new ClientRestrictions(
            default!,
            Limit.PerRequest(ids.Count + 1),
            default!,
            default!,
            default!,
            default!);
        _idCollectionValidator.Validate(ids).Returns(Validation.ValidResult);
        _clientEntityService.GetById(borrowerId).Returns(borrower);
        _employeeEntityService.GetById(lenderId).Returns(lender);
        _restrictionsService.GetRestrictionsForAccount(borrowerAccountId)
            .Returns(Result.Valid(clientRestrictions));
        _entityService.Insert(Arg.Any<BorrowRecord>(), Arg.Any<IValidator<BorrowRecord>>())
            .Returns(Result.Invalid());

        var success = _borrowService.Borrow(borrowerId, lenderId, ids);

        Assert.IsFalse(success);
    }

    [TestMethod]
    public void Borrow_ShouldReturnFalse_WhenBorrowRequestExcceedsLimit()
    {
        var borrowerId = 1;
        var lenderId = 4;
        var ids = new List<int>() { 1, 4, 5, 7, 9 }.ToIdCollection();
        var borrowerAccountId = 21;
        var borrower = new Client(borrowerAccountId) { Id = borrowerId };
        var lenderAccountId = 32;
        var lender = new Employee(lenderAccountId) { Id = lenderId };
        var clientRestrictions = new ClientRestrictions(
            default!,
            Limit.PerRequest(ids.Count - 3),
            default!,
            default!,
            default!,
            default!);
        _idCollectionValidator.Validate(ids).Returns(Validation.ValidResult);
        _clientEntityService.GetById(borrowerId).Returns(borrower);
        _employeeEntityService.GetById(lenderId).Returns(lender);
        _restrictionsService.GetRestrictionsForAccount(borrowerAccountId)
            .Returns(Result.Valid(clientRestrictions));
        _entityService.Insert(Arg.Any<BorrowRecord>(), Arg.Any<IValidator<BorrowRecord>>())
            .Returns(call => Result.Valid(call.Arg<BorrowRecord>()));

        var success = _borrowService.Borrow(borrowerId, lenderId, ids);

        Assert.IsFalse(success);
    }

    [TestMethod]
    public void Borrow_ShouldReturnFalse_WhenGetClientRestrictionsReturnInvalid()
    {
        var borrowerId = 1;
        var lenderId = 4;
        var ids = new List<int>() { 1, 4, 5, 7, 9 }.ToIdCollection();
        var borrowerAccountId = 21;
        var borrower = new Client(borrowerAccountId) { Id = borrowerId };
        var lenderAccountId = 32;
        var lender = new Employee(lenderAccountId) { Id = lenderId };
        var clientRestrictions = new ClientRestrictions(
            default!,
            Limit.PerRequest(ids.Count + 1),
            default!,
            default!,
            default!,
            default!);
        _idCollectionValidator.Validate(ids).Returns(Validation.ValidResult);
        _clientEntityService.GetById(borrowerId).Returns(borrower);
        _employeeEntityService.GetById(lenderId).Returns(lender);
        _restrictionsService.GetRestrictionsForAccount(borrowerAccountId)
            .Returns(Result.Invalid());
        _entityService.Insert(Arg.Any<BorrowRecord>(), Arg.Any<IValidator<BorrowRecord>>())
            .Returns(call => Result.Valid(call.Arg<BorrowRecord>()));

        var success = _borrowService.Borrow(borrowerId, lenderId, ids);

        Assert.IsFalse(success);
    }

    [TestMethod]
    public void Borrow_ShouldReturnFalse_WhenLenderIsNotFound()
    {
        var borrowerId = 1;
        var lenderId = 4;
        var ids = new List<int>() { 1, 4, 5, 7, 9 }.ToIdCollection();
        var borrowerAccountId = 21;
        var borrower = new Client(borrowerAccountId) { Id = borrowerId };
        var clientRestrictions = new ClientRestrictions(
            default!,
            Limit.PerRequest(ids.Count + 1),
            default!,
            default!,
            default!,
            default!);
        _idCollectionValidator.Validate(ids).Returns(Validation.ValidResult);
        _clientEntityService.GetById(borrowerId).Returns(borrower);
        _employeeEntityService.GetById(lenderId).Returns((Employee?)null);
        _restrictionsService.GetRestrictionsForAccount(borrowerAccountId)
            .Returns(Result.Valid(clientRestrictions));
        _entityService.Insert(Arg.Any<BorrowRecord>(), Arg.Any<IValidator<BorrowRecord>>())
            .Returns(call => Result.Valid(call.Arg<BorrowRecord>()));

        var success = _borrowService.Borrow(borrowerId, lenderId, ids);

        Assert.IsFalse(success);
    }

    [TestMethod]
    public void Borrow_ShouldReturnFalse_WhenBorrowerIsNotFound()
    {
        var borrowerId = 1;
        var lenderId = 4;
        var ids = new List<int>() { 1, 4, 5, 7, 9 }.ToIdCollection();
        var borrowerAccountId = 21;
        var lenderAccountId = 32;
        var lender = new Employee(lenderAccountId) { Id = lenderId };
        var clientRestrictions = new ClientRestrictions(
            default!,
            Limit.PerRequest(ids.Count + 1),
            default!,
            default!,
            default!,
            default!);
        _idCollectionValidator.Validate(ids).Returns(Validation.ValidResult);
        _clientEntityService.GetById(borrowerId).Returns((Client?)null);
        _employeeEntityService.GetById(lenderId).Returns(lender);
        _restrictionsService.GetRestrictionsForAccount(borrowerAccountId)
            .Returns(Result.Valid(clientRestrictions));
        _entityService.Insert(Arg.Any<BorrowRecord>(), Arg.Any<IValidator<BorrowRecord>>())
            .Returns(call => Result.Valid(call.Arg<BorrowRecord>()));

        var success = _borrowService.Borrow(borrowerId, lenderId, ids);

        Assert.IsFalse(success);
    }

    [TestMethod]
    public void Borrow_ShouldReturnFalse_WhenIdValidationDoesNotPass()
    {
        var borrowerId = 1;
        var lenderId = 4;
        var ids = new List<int>() { 1, 4, 5, 7, 9 }.ToIdCollection();
        var borrowerAccountId = 21;
        var borrower = new Client(borrowerAccountId) { Id = borrowerId };
        var lenderAccountId = 32;
        var lender = new Employee(lenderAccountId) { Id = lenderId };
        var clientRestrictions = new ClientRestrictions(
            default!,
            Limit.PerRequest(ids.Count + 1),
            default!,
            default!,
            default!,
            default!);
        _idCollectionValidator.Validate(ids).Returns(Validation.InvalidResult);
        _clientEntityService.GetById(borrowerId).Returns(borrower);
        _employeeEntityService.GetById(lenderId).Returns(lender);
        _restrictionsService.GetRestrictionsForAccount(borrowerAccountId)
            .Returns(Result.Valid(clientRestrictions));
        _entityService.Insert(Arg.Any<BorrowRecord>(), Arg.Any<IValidator<BorrowRecord>>())
            .Returns(call => Result.Valid(call.Arg<BorrowRecord>()));

        var success = _borrowService.Borrow(borrowerId, lenderId, ids);

        Assert.IsFalse(success);
    }

    [TestMethod]
    public void Borrow_ShouldReturnTrueAndInsertCorrectBorrowRecord_WhenAllValidationsPass()
    {
        var borrowerId = 1;
        var lenderId = 4;
        var ids = new List<int>() { 1, 4, 5, 7, 9 }.ToIdCollection();
        var borrowerAccountId = 21;
        var borrower = new Client(borrowerAccountId) { Id = borrowerId };
        var lenderAccountId = 32;
        var lender = new Employee(lenderAccountId) { Id = lenderId };
        var clientRestrictions = new ClientRestrictions(
            default!,
            Limit.PerRequest(ids.Count + 1),
            default!,
            default!,
            default!,
            default!);
        BorrowRecord? borrowRecord = null;
        _idCollectionValidator.Validate(ids).Returns(Validation.ValidResult);
        _clientEntityService.GetById(borrowerId).Returns(borrower);
        _employeeEntityService.GetById(lenderId).Returns(lender);
        _restrictionsService.GetRestrictionsForAccount(borrowerAccountId)
            .Returns(Result.Valid(clientRestrictions));
        _entityService.Insert(
            Arg.Do<BorrowRecord>(x => borrowRecord = x),
            Arg.Any<IValidator<BorrowRecord>>())
            .Returns(call => Result.Valid(call.Arg<BorrowRecord>()));

        var success = _borrowService.Borrow(borrowerId, lenderId, ids);

        Assert.IsTrue(success);
        Assert.IsNotNull(borrowRecord);
        Assert.AreEqual(borrowerId, borrowRecord.BorrowerId);
        Assert.AreEqual(lenderId, borrowRecord.LenderId);
    }

    [TestMethod]
    public void Borrow_ShouldReturnTrue_WhenAllValidationsPass()
    {
        var borrowerId = 1;
        var lenderId = 4;
        var ids = new List<int>() { 1, 4, 5, 7, 9 }.ToIdCollection();
        var borrowerAccountId = 21;
        var borrower = new Client(borrowerAccountId) { Id = borrowerId };
        var lenderAccountId = 32;
        var lender = new Employee(lenderAccountId) { Id = lenderId };
        var clientRestrictions = new ClientRestrictions(
            default!,
            Limit.PerRequest(ids.Count + 1),
            default!,
            default!,
            default!,
            default!);
        _idCollectionValidator.Validate(ids).Returns(Validation.ValidResult);
        _clientEntityService.GetById(borrowerId).Returns(borrower);
        _employeeEntityService.GetById(lenderId).Returns(lender);
        _restrictionsService.GetRestrictionsForAccount(borrowerAccountId)
            .Returns(Result.Valid(clientRestrictions));
        _entityService.Insert(Arg.Any<BorrowRecord>(), Arg.Any<IValidator<BorrowRecord>>())
            .Returns(call => Result.Valid(call.Arg<BorrowRecord>()));

        var success = _borrowService.Borrow(borrowerId, lenderId, ids);

        Assert.IsTrue(success);
    }
}