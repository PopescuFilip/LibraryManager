using DomainModel;
using DomainModel.DTOs;
using DomainModel.Restrictions;
using FluentValidation;
using FluentValidation.Results;
using NSubstitute;
using ServiceLayer.Borrowing;
using ServiceLayer.CRUD;
using ServiceLayer.Restriction;
using ServiceLayer.UnitTests.TestHelpers;
using System.Collections.Immutable;
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
    private IBookQueryService _bookQueryService = default!;
    private IDomainQueryService _domainQueryService = default!;

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
        _bookQueryService = Substitute.For<IBookQueryService>();
        _domainQueryService = Substitute.For<IDomainQueryService>();

        _borrowService = new BorrowService(
            _entityService,
            _clientEntityService,
            _employeeEntityService,
            _idCollectionValidator,
            _restrictionsService,
            _employeeRestrictionsProvider,
            _borrowRecordQueryService,
            _bookQueryService,
            _domainQueryService
            );
    }

    [TestMethod]
    public void Borrow_ShouldReturnFalse_WhenInsertFails()
    {
        var borrowerId = 1;
        var lenderId = 4;
        var ids = new List<int>() { 1, 4, 5, 7, 9 }.ToIdCollection();
        var options = Generator.GenerateBorrowOptionsFrom(ids, DateTime.Now.AddDays(3));
        var borrowerAccountId = 21;
        var borrower = new Client(borrowerAccountId) { Id = borrowerId };
        var lenderAccountId = 32;
        var lender = new Employee(lenderAccountId) { Id = lenderId };
        var clientRestrictions = new ClientRestrictions(
            Limit.PerPeriodInDays(30, 4),
            Limit.PerRequest(ids.Count + 1),
            Limit.PerPeriodInMonths(12, 1),
            default!,
            Limit.PerPeriodInDays(1, 10),
            Limit.PerDay(10));
        var borrowedBooksGiven = 3;
        var employeeRestrictions = new EmployeeRestrictions(
            Limit.PerDay(ids.Count + borrowedBooksGiven + 21)
            );
        var booksBorrowedInPeriod =
            clientRestrictions.BorrowedBooksLimit.ItemCount - ids.Count - 2;
        var booksBorrowedToday =
            clientRestrictions.BorrowedBooksPerDayLimit.ItemCount - ids.Count - 2;
        var bookDetails = ids
            .Select(id => new BookDetails(
                id,
                new Book(BookStatus.Available, id) { Id = 1 },
                id,
                51, 51,
                [id]))
            .ToList();
        var parentDomainIds = new List<BookParentDomainIds>()
        {
            new(ids.Take(2)),
            new(ids.Take(3)),
            new(ids.Take(2)),
        };
        _idCollectionValidator.Validate(Arg.Is<IdCollection>(x => x.SequenceEqual(ids)))
            .Returns(Validation.ValidResult);
        _clientEntityService.GetById(borrowerId).Returns(borrower);
        _employeeEntityService.GetById(lenderId).Returns(lender);
        _restrictionsService.GetRestrictionsForAccount(borrowerAccountId)
            .Returns(Result.Valid(clientRestrictions));
        _borrowRecordQueryService.GetBooksLendedTodayCount(lenderId)
            .Returns(borrowedBooksGiven);
        _borrowRecordQueryService.GetBooksBorrowedTodayCount(borrowerId)
            .Returns(booksBorrowedToday);
        _borrowRecordQueryService
            .GetBooksBorrowedInPeriodCount(borrowerId, Arg.Any<int>(), Arg.Any<DateTime>(), Arg.Any<DateTime>())
            .Returns(0);
        _employeeRestrictionsProvider.Get().Returns(Result.Valid(employeeRestrictions));
        _borrowRecordQueryService
            .GetBooksBorrowedInPeriodCount(borrowerId, Arg.Any<DateTime>(), Arg.Any<DateTime>())
            .Returns(booksBorrowedInPeriod);
        _bookQueryService.GetBookDetails(Arg.Is<IdCollection>(x => x.SequenceEqual(ids)))
            .Returns(bookDetails);
        _domainQueryService.GetParentIds(Arg.Is<IEnumerable<int>>(x =>
                x.SequenceEqual(bookDetails.SelectDomainIds())))
            .Returns(bookDetails.SelectDomainIds());
        _borrowRecordQueryService
            .GetDomainIdsForBorrowedInPeriod(borrowerId, Arg.Any<DateTime>(), Arg.Any<DateTime>())
            .Returns([]);
        _domainQueryService.GetParentDomainIds(Arg.Any<IEnumerable<BookDomainIds>>())
            .Returns(parentDomainIds);
        _entityService
            .InsertRange(Arg.Any<IReadOnlyCollection<BorrowRecord>>(), Arg.Any<IValidator<BorrowRecord>>())
            .Returns(false);

        var success = _borrowService.Borrow(borrowerId, lenderId, options);

        Assert.IsFalse(success);
    }

    [TestMethod]
    public void Borrow_ShouldReturnFalse_WhenLenderWouldExceedLendLimit()
    {
        var borrowerId = 1;
        var lenderId = 4;
        var ids = new List<int>() { 1, 4, 5, 7, 9 }.ToIdCollection();
        var options = Generator.GenerateBorrowOptionsFrom(ids, DateTime.Now.AddDays(3));
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
        var borrowedBooksGiven = 3;
        var employeeRestrictions = new EmployeeRestrictions(
            Limit.PerDay(ids.Count + borrowedBooksGiven - 2)
            );
        _idCollectionValidator.Validate(Arg.Is<IdCollection>(x => x.SequenceEqual(ids)))
            .Returns(Validation.ValidResult);
        _clientEntityService.GetById(borrowerId).Returns(borrower);
        _employeeEntityService.GetById(lenderId).Returns(lender);
        _restrictionsService.GetRestrictionsForAccount(borrowerAccountId)
            .Returns(Result.Valid(clientRestrictions));
        _borrowRecordQueryService.GetBooksLendedTodayCount(lenderId).Returns(borrowedBooksGiven);
        _employeeRestrictionsProvider.Get().Returns(Result.Valid(employeeRestrictions));
        _entityService.Insert(Arg.Any<BorrowRecord>(), Arg.Any<IValidator<BorrowRecord>>())
            .Returns(call => Result.Valid(call.Arg<BorrowRecord>()));

        var success = _borrowService.Borrow(borrowerId, lenderId, options);

        Assert.IsFalse(success);
    }

    [TestMethod]
    public void Borrow_ShouldReturnFalse_WhenGetEmployeeRestrictionsReturnsInvalid()
    {
        var borrowerId = 1;
        var lenderId = 4;
        var ids = new List<int>() { 1, 4, 5, 7, 9 }.ToIdCollection();
        var options = Generator.GenerateBorrowOptionsFrom(ids, DateTime.Now.AddDays(3));
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
        var borrowedBooksGiven = 3;
        _idCollectionValidator.Validate(Arg.Is<IdCollection>(x => x.SequenceEqual(ids)))
            .Returns(Validation.ValidResult);
        _clientEntityService.GetById(borrowerId).Returns(borrower);
        _employeeEntityService.GetById(lenderId).Returns(lender);
        _restrictionsService.GetRestrictionsForAccount(borrowerAccountId)
            .Returns(Result.Valid(clientRestrictions));
        _borrowRecordQueryService.GetBooksLendedTodayCount(lenderId).Returns(borrowedBooksGiven);
        _employeeRestrictionsProvider.Get().Returns(Result.Invalid());
        _entityService.Insert(Arg.Any<BorrowRecord>(), Arg.Any<IValidator<BorrowRecord>>())
            .Returns(call => Result.Valid(call.Arg<BorrowRecord>()));

        var success = _borrowService.Borrow(borrowerId, lenderId, options);

        Assert.IsFalse(success);
    }

    [TestMethod]
    public void Borrow_ShouldReturnFalse_WhenBorrowRequestExcceedsLimit()
    {
        var borrowerId = 1;
        var lenderId = 4;
        var ids = new List<int>() { 1, 4, 5, 7, 9 }.ToIdCollection();
        var options = Generator.GenerateBorrowOptionsFrom(ids, DateTime.Now.AddDays(3));
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
        _idCollectionValidator.Validate(Arg.Is<IdCollection>(x => x.SequenceEqual(ids)))
            .Returns(Validation.ValidResult);
        _clientEntityService.GetById(borrowerId).Returns(borrower);
        _employeeEntityService.GetById(lenderId).Returns(lender);
        _restrictionsService.GetRestrictionsForAccount(borrowerAccountId)
            .Returns(Result.Valid(clientRestrictions));
        _entityService.Insert(Arg.Any<BorrowRecord>(), Arg.Any<IValidator<BorrowRecord>>())
            .Returns(call => Result.Valid(call.Arg<BorrowRecord>()));

        var success = _borrowService.Borrow(borrowerId, lenderId, options);

        Assert.IsFalse(success);
    }

    [TestMethod]
    public void Borrow_ShouldReturnFalse_WhenGetClientRestrictionsReturnInvalid()
    {
        var borrowerId = 1;
        var lenderId = 4;
        var ids = new List<int>() { 1, 4, 5, 7, 9 }.ToIdCollection();
        var options = Generator.GenerateBorrowOptionsFrom(ids, DateTime.Now.AddDays(3));
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
        _idCollectionValidator.Validate(Arg.Is<IdCollection>(x => x.SequenceEqual(ids)))
            .Returns(Validation.ValidResult);
        _clientEntityService.GetById(borrowerId).Returns(borrower);
        _employeeEntityService.GetById(lenderId).Returns(lender);
        _restrictionsService.GetRestrictionsForAccount(borrowerAccountId)
            .Returns(Result.Invalid());
        _entityService.Insert(Arg.Any<BorrowRecord>(), Arg.Any<IValidator<BorrowRecord>>())
            .Returns(call => Result.Valid(call.Arg<BorrowRecord>()));

        var success = _borrowService.Borrow(borrowerId, lenderId, options);

        Assert.IsFalse(success);
    }

    [TestMethod]
    public void Borrow_ShouldReturnFalse_WhenLenderIsNotFound()
    {
        var borrowerId = 1;
        var lenderId = 4;
        var ids = new List<int>() { 1, 4, 5, 7, 9 }.ToIdCollection();
        var options = Generator.GenerateBorrowOptionsFrom(ids, DateTime.Now.AddDays(3));
        var borrowerAccountId = 21;
        var borrower = new Client(borrowerAccountId) { Id = borrowerId };
        var clientRestrictions = new ClientRestrictions(
            default!,
            Limit.PerRequest(ids.Count + 1),
            default!,
            default!,
            default!,
            default!);
        _idCollectionValidator.Validate(Arg.Is<IdCollection>(x => x.SequenceEqual(ids)))
            .Returns(Validation.ValidResult);
        _clientEntityService.GetById(borrowerId).Returns(borrower);
        _employeeEntityService.GetById(lenderId).Returns((Employee?)null);
        _restrictionsService.GetRestrictionsForAccount(borrowerAccountId)
            .Returns(Result.Valid(clientRestrictions));
        _entityService.Insert(Arg.Any<BorrowRecord>(), Arg.Any<IValidator<BorrowRecord>>())
            .Returns(call => Result.Valid(call.Arg<BorrowRecord>()));

        var success = _borrowService.Borrow(borrowerId, lenderId, options);

        Assert.IsFalse(success);
    }

    [TestMethod]
    public void Borrow_ShouldReturnFalse_WhenBorrowerIsNotFound()
    {
        var borrowerId = 1;
        var lenderId = 4;
        var ids = new List<int>() { 1, 4, 5, 7, 9 }.ToIdCollection();
        var options = Generator.GenerateBorrowOptionsFrom(ids, DateTime.Now.AddDays(3));
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
        _idCollectionValidator.Validate(Arg.Is<IdCollection>(x => x.SequenceEqual(ids)))
            .Returns(Validation.ValidResult);
        _clientEntityService.GetById(borrowerId).Returns((Client?)null);
        _employeeEntityService.GetById(lenderId).Returns(lender);
        _restrictionsService.GetRestrictionsForAccount(borrowerAccountId)
            .Returns(Result.Valid(clientRestrictions));
        _entityService.Insert(Arg.Any<BorrowRecord>(), Arg.Any<IValidator<BorrowRecord>>())
            .Returns(call => Result.Valid(call.Arg<BorrowRecord>()));

        var success = _borrowService.Borrow(borrowerId, lenderId, options);

        Assert.IsFalse(success);
    }

    [TestMethod]
    public void Borrow_ShouldReturnFalse_WhenIdValidationDoesNotPass()
    {
        var borrowerId = 1;
        var lenderId = 4;
        var ids = new List<int>() { 1, 4, 5, 7, 9 }.ToIdCollection();
        var options = Generator.GenerateBorrowOptionsFrom(ids, DateTime.Now.AddDays(3));
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
        _idCollectionValidator.Validate(Arg.Is<IdCollection>(x => x.SequenceEqual(ids)))
            .Returns(Validation.InvalidResult);
        _clientEntityService.GetById(borrowerId).Returns(borrower);
        _employeeEntityService.GetById(lenderId).Returns(lender);
        _restrictionsService.GetRestrictionsForAccount(borrowerAccountId)
            .Returns(Result.Valid(clientRestrictions));
        _entityService.Insert(Arg.Any<BorrowRecord>(), Arg.Any<IValidator<BorrowRecord>>())
            .Returns(call => Result.Valid(call.Arg<BorrowRecord>()));

        var success = _borrowService.Borrow(borrowerId, lenderId, options);

        Assert.IsFalse(success);
    }

    [TestMethod]
    public void Borrow_ShouldReturnTrueAndInsertCorrectBorrowRecord_WhenAllValidationsPass()
    {
        List<BorrowRecord>? borrowRecords = null;
        var borrowerId = 1;
        var lenderId = 4;
        var ids = new List<int>() { 1, 4, 5, 7, 9 }.ToIdCollection();
        var options = Generator.GenerateBorrowOptionsFrom(ids, DateTime.Now.AddDays(3));
        var borrowerAccountId = 21;
        var borrower = new Client(borrowerAccountId) { Id = borrowerId };
        var lenderAccountId = 32;
        var lender = new Employee(lenderAccountId) { Id = lenderId };
        var clientRestrictions = new ClientRestrictions(
            Limit.PerPeriodInDays(30, 4),
            Limit.PerRequest(ids.Count + 1),
            Limit.PerPeriodInMonths(12, 1),
            default!,
            Limit.PerPeriodInDays(1, 10),
            Limit.PerDay(10));
        var borrowedBooksGiven = 3;
        var employeeRestrictions = new EmployeeRestrictions(
            Limit.PerDay(ids.Count + borrowedBooksGiven + 21)
            );
        var booksBorrowedInPeriod =
            clientRestrictions.BorrowedBooksLimit.ItemCount - ids.Count - 2;
        var booksBorrowedToday =
            clientRestrictions.BorrowedBooksPerDayLimit.ItemCount - ids.Count - 2;
        var bookDetails = ids
            .Select(id => new BookDetails(
                id,
                new Book(BookStatus.Available, id) { Id = 1 },
                id,
                51, 51,
                [id]))
            .ToList();
        var parentDomainIds = new List<BookParentDomainIds>()
        {
            new(ids.Take(2)),
            new(ids.Take(3)),
            new(ids.Take(2)),
        };
        _idCollectionValidator.Validate(Arg.Is<IdCollection>(x => x.SequenceEqual(ids)))
            .Returns(Validation.ValidResult);
        _clientEntityService.GetById(borrowerId).Returns(borrower);
        _employeeEntityService.GetById(lenderId).Returns(lender);
        _restrictionsService.GetRestrictionsForAccount(borrowerAccountId)
            .Returns(Result.Valid(clientRestrictions));
        _borrowRecordQueryService.GetBooksLendedTodayCount(lenderId)
            .Returns(borrowedBooksGiven);
        _borrowRecordQueryService.GetBooksBorrowedTodayCount(borrowerId)
            .Returns(booksBorrowedToday);
        _borrowRecordQueryService
            .GetBooksBorrowedInPeriodCount(borrowerId, Arg.Any<int>(), Arg.Any<DateTime>(), Arg.Any<DateTime>())
            .Returns(0);
        _employeeRestrictionsProvider.Get().Returns(Result.Valid(employeeRestrictions));
        _borrowRecordQueryService
            .GetBooksBorrowedInPeriodCount(borrowerId, Arg.Any<DateTime>(), Arg.Any<DateTime>())
            .Returns(booksBorrowedInPeriod);
        _bookQueryService.GetBookDetails(Arg.Is<IdCollection>(x => x.SequenceEqual(ids)))
            .Returns(bookDetails);
        _domainQueryService.GetParentIds(Arg.Is<IEnumerable<int>>(x =>
                x.SequenceEqual(bookDetails.SelectDomainIds())))
            .Returns(bookDetails.SelectDomainIds());
        _borrowRecordQueryService
            .GetDomainIdsForBorrowedInPeriod(borrowerId, Arg.Any<DateTime>(), Arg.Any<DateTime>())
            .Returns([]);
        _domainQueryService.GetParentDomainIds(Arg.Any<IEnumerable<BookDomainIds>>())
            .Returns(parentDomainIds);
        _entityService.InsertRange(
            Arg.Do<IReadOnlyCollection<BorrowRecord>>(x => borrowRecords = [.. x]),
            Arg.Any<IValidator<BorrowRecord>>())
            .Returns(true);

        var success = _borrowService.Borrow(borrowerId, lenderId, options);

        Assert.IsTrue(success);
        Assert.IsNotNull(borrowRecords);
        Assert.IsTrue(borrowRecords.All(x => x.BorrowerId == borrowerId));
        Assert.IsTrue(borrowRecords.All(x => x.LenderId == lenderId));
    }

    [TestMethod]
    public void Borrow_ShouldReturnTrue_WhenAllValidationsPass()
    {
        var borrowerId = 1;
        var lenderId = 4;
        var ids = new List<int>() { 1, 4, 5, 7, 9 }.ToIdCollection();
        var options = Generator.GenerateBorrowOptionsFrom(ids, DateTime.Now.AddDays(3));
        var borrowerAccountId = 21;
        var borrower = new Client(borrowerAccountId) { Id = borrowerId };
        var lenderAccountId = 32;
        var lender = new Employee(lenderAccountId) { Id = lenderId };
        var clientRestrictions = new ClientRestrictions(
            Limit.PerPeriodInDays(30, 4),
            Limit.PerRequest(ids.Count + 1),
            Limit.PerPeriodInMonths(12, 1),
            default!,
            Limit.PerPeriodInDays(1, 10),
            Limit.PerDay(10));
        var borrowedBooksGiven = 3;
        var employeeRestrictions = new EmployeeRestrictions(
            Limit.PerDay(ids.Count + borrowedBooksGiven + 21)
            );
        var booksBorrowedInPeriod =
            clientRestrictions.BorrowedBooksLimit.ItemCount - ids.Count - 2;
        var booksBorrowedToday =
            clientRestrictions.BorrowedBooksPerDayLimit.ItemCount - ids.Count - 2;
        var bookDetails = ids
            .Select(id => new BookDetails(
                id,
                new Book(BookStatus.Available, id) { Id = 1 },
                id,
                51, 51,
                [id]))
            .ToList();
        var parentDomainIds = new List<BookParentDomainIds>()
        {
            new(ids.Take(2)),
            new(ids.Take(3)),
            new(ids.Take(2)),
        };
        _idCollectionValidator.Validate(Arg.Is<IdCollection>(x => x.SequenceEqual(ids)))
            .Returns(Validation.ValidResult);
        _clientEntityService.GetById(borrowerId).Returns(borrower);
        _employeeEntityService.GetById(lenderId).Returns(lender);
        _restrictionsService.GetRestrictionsForAccount(borrowerAccountId)
            .Returns(Result.Valid(clientRestrictions));
        _borrowRecordQueryService.GetBooksLendedTodayCount(lenderId)
            .Returns(borrowedBooksGiven);
        _borrowRecordQueryService.GetBooksBorrowedTodayCount(borrowerId)
            .Returns(booksBorrowedToday);
        _borrowRecordQueryService
            .GetBooksBorrowedInPeriodCount(borrowerId, Arg.Any<int>(), Arg.Any<DateTime>(), Arg.Any<DateTime>())
            .Returns(0);
        _employeeRestrictionsProvider.Get().Returns(Result.Valid(employeeRestrictions));
        _borrowRecordQueryService
            .GetBooksBorrowedInPeriodCount(borrowerId, Arg.Any<DateTime>(), Arg.Any<DateTime>())
            .Returns(booksBorrowedInPeriod);
        _bookQueryService.GetBookDetails(Arg.Is<IdCollection>(x => x.SequenceEqual(ids)))
            .Returns(bookDetails);
        _domainQueryService.GetParentIds(Arg.Is<IEnumerable<int>>(x =>
                x.SequenceEqual(bookDetails.SelectDomainIds())))
            .Returns(bookDetails.SelectDomainIds());
        _borrowRecordQueryService
            .GetDomainIdsForBorrowedInPeriod(borrowerId, Arg.Any<DateTime>(), Arg.Any<DateTime>())
            .Returns([]);
        _domainQueryService.GetParentDomainIds(Arg.Any<IEnumerable<BookDomainIds>>())
            .Returns(parentDomainIds);
        _entityService
            .InsertRange(Arg.Any<IReadOnlyCollection<BorrowRecord>>(), Arg.Any<IValidator<BorrowRecord>>())
            .Returns(true);

        var success = _borrowService.Borrow(borrowerId, lenderId, options);

        Assert.IsTrue(success);
    }
}