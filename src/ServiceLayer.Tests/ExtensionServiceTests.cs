using DomainModel;
using DomainModel.Restrictions;
using FluentValidation;
using NSubstitute;
using ServiceLayer.Borrowing;
using ServiceLayer.CRUD;
using ServiceLayer.Restriction;
using System.Diagnostics.CodeAnalysis;

namespace ServiceLayer.UnitTests;

[TestClass]
[ExcludeFromCodeCoverage]
public class ExtensionServiceTests
{
    private ExtensionService _extensionService = default!;
    private IEntityService<Extension> _entityService = default!;
    private IEntityService<Client> _clientEntityService = default!;
    private IRestrictionsService _restrictionsService = default!;
    private IExtensionQueryService _extensionQueryService = default!;
    private IBorrowRecordQueryService _borrowRecordQueryService = default!;
    private IValidator<Extension> _validator = default!;

    [TestInitialize]
    public void Init()
    {
        _entityService = Substitute.For<IEntityService<Extension>>();
        _clientEntityService = Substitute.For<IEntityService<Client>>();
        _restrictionsService = Substitute.For<IRestrictionsService>();
        _extensionQueryService = Substitute.For<IExtensionQueryService>();
        _borrowRecordQueryService = Substitute.For<IBorrowRecordQueryService>();
        _validator = Substitute.For<IValidator<Extension>>();

        _extensionService = new ExtensionService(
            _entityService,
            _clientEntityService,
            _restrictionsService,
            _extensionQueryService,
            _borrowRecordQueryService,
            _validator);
    }

    [TestMethod]
    public void Extend_ShouldReturnFalse_WhenInsertFails()
    {
        var borrowerId = 2;
        var bookId = 3;
        var extendDaysCount = 5;
        var borrowerAccountId = 4;
        var borrower = new Client(borrowerAccountId) { Id = borrowerId };
        var initialBorrowedUntil = DateTime.Now.AddDays(2);
        var borrowRecord = new BorrowRecord(borrowerId, 34, bookId, initialBorrowedUntil)
        {
            BorrowedBook = new Book(BookStatus.Borrowed, 31)
            {
                Id = bookId,
                BorrowedById = borrowerId
            }
        };
        var clientRestrictions = new ClientRestrictions(
            default!,
            default!,
            default!,
            Limit.PerPeriodInMonths(10, 4),
            default!,
            default!);
        var extensionDaysForPeriod =
            clientRestrictions.ExtensionDaysLimit.ItemCount - extendDaysCount - 2;

        _clientEntityService.GetById(borrowerId).Returns(borrower);
        _borrowRecordQueryService.GetActiveBorrowRecordWithBook(borrowerId, bookId)
            .Returns(borrowRecord);
        _restrictionsService.GetRestrictionsForAccount(borrowerAccountId)
            .Returns(Result.Valid(clientRestrictions));
        _extensionQueryService
            .GetExtensionDaysForPeriod(borrowerId, Arg.Any<DateTime>(), Arg.Any<DateTime>())
            .Returns(extensionDaysForPeriod);
        _entityService.Insert(Arg.Any<Extension>(), _validator)
            .Returns(Result.Invalid());

        var success = _extensionService.Extend(borrowerId, bookId, extendDaysCount);

        Assert.IsFalse(success);
    }

    [TestMethod]
    public void Extend_ShouldReturnFalse_WhenLimitIsExceeded()
    {
        var borrowerId = 2;
        var bookId = 3;
        var extendDaysCount = 5;
        var borrowerAccountId = 4;
        var borrower = new Client(borrowerAccountId) { Id = borrowerId };
        var initialBorrowedUntil = DateTime.Now.AddDays(2);
        var borrowRecord = new BorrowRecord(borrowerId, 34, bookId, initialBorrowedUntil)
        {
            BorrowedBook = new Book(BookStatus.Borrowed, 31)
            {
                Id = bookId,
                BorrowedById = borrowerId
            }
        };
        var clientRestrictions = new ClientRestrictions(
            default!,
            default!,
            default!,
            Limit.PerPeriodInMonths(10, 4),
            default!,
            default!);
        var extensionDaysForPeriod =
            clientRestrictions.ExtensionDaysLimit.ItemCount - extendDaysCount + 2;

        _clientEntityService.GetById(borrowerId).Returns(borrower);
        _borrowRecordQueryService.GetActiveBorrowRecordWithBook(borrowerId, bookId)
            .Returns(borrowRecord);
        _restrictionsService.GetRestrictionsForAccount(borrowerAccountId)
            .Returns(Result.Valid(clientRestrictions));
        _extensionQueryService
            .GetExtensionDaysForPeriod(borrowerId, Arg.Any<DateTime>(), Arg.Any<DateTime>())
            .Returns(extensionDaysForPeriod);
        _entityService.Insert(Arg.Any<Extension>(), _validator)
            .Returns(call => Result.Valid(call.Arg<Extension>()));

        var success = _extensionService.Extend(borrowerId, bookId, extendDaysCount);

        Assert.IsFalse(success);
    }

    [TestMethod]
    public void Extend_ShouldReturnFalse_WhenRestrictionsAreNotFound()
    {
        var borrowerId = 2;
        var bookId = 3;
        var extendDaysCount = 5;
        var borrowerAccountId = 4;
        var borrower = new Client(borrowerAccountId) { Id = borrowerId };
        var initialBorrowedUntil = DateTime.Now.AddDays(2);
        var borrowRecord = new BorrowRecord(borrowerId, 34, bookId, initialBorrowedUntil)
        {
            BorrowedBook = new Book(BookStatus.Borrowed, 31)
            {
                Id = bookId,
                BorrowedById = borrowerId
            }
        };
        var clientRestrictions = new ClientRestrictions(
            default!,
            default!,
            default!,
            Limit.PerPeriodInMonths(10, 4),
            default!,
            default!);
        var extensionDaysForPeriod =
            clientRestrictions.ExtensionDaysLimit.ItemCount - extendDaysCount - 2;

        _clientEntityService.GetById(borrowerId).Returns(borrower);
        _borrowRecordQueryService.GetActiveBorrowRecordWithBook(borrowerId, bookId)
            .Returns(borrowRecord);
        _restrictionsService.GetRestrictionsForAccount(borrowerAccountId)
            .Returns(Result.Invalid());
        _extensionQueryService
            .GetExtensionDaysForPeriod(borrowerId, Arg.Any<DateTime>(), Arg.Any<DateTime>())
            .Returns(extensionDaysForPeriod);
        _entityService.Insert(Arg.Any<Extension>(), _validator)
            .Returns(call => Result.Valid(call.Arg<Extension>()));

        var success = _extensionService.Extend(borrowerId, bookId, extendDaysCount);

        Assert.IsFalse(success);
    }

    [TestMethod]
    public void Extend_ShouldReturnFalse_WhenBorrowerIdForBookDoesNotMatchRequester()
    {
        var borrowerId = 2;
        var bookId = 3;
        var extendDaysCount = 5;
        var borrowerAccountId = 4;
        var borrower = new Client(borrowerAccountId) { Id = borrowerId };
        var initialBorrowedUntil = DateTime.Now.AddDays(2);
        var borrowRecord = new BorrowRecord(borrowerId, 34, bookId, initialBorrowedUntil)
        {
            BorrowedBook = new Book(BookStatus.Borrowed, 31)
            {
                Id = bookId,
                BorrowedById = 123
            }
        };
        var clientRestrictions = new ClientRestrictions(
            default!,
            default!,
            default!,
            Limit.PerPeriodInMonths(10, 4),
            default!,
            default!);
        var extensionDaysForPeriod =
            clientRestrictions.ExtensionDaysLimit.ItemCount - extendDaysCount - 2;

        _clientEntityService.GetById(borrowerId).Returns(borrower);
        _borrowRecordQueryService.GetActiveBorrowRecordWithBook(borrowerId, bookId)
            .Returns(borrowRecord);
        _restrictionsService.GetRestrictionsForAccount(borrowerAccountId)
            .Returns(Result.Valid(clientRestrictions));
        _extensionQueryService
            .GetExtensionDaysForPeriod(borrowerId, Arg.Any<DateTime>(), Arg.Any<DateTime>())
            .Returns(extensionDaysForPeriod);
        _entityService.Insert(Arg.Any<Extension>(), _validator)
            .Returns(call => Result.Valid(call.Arg<Extension>()));

        var success = _extensionService.Extend(borrowerId, bookId, extendDaysCount);

        Assert.IsFalse(success);
    }

    [TestMethod]
    public void Extend_ShouldReturnFalse_WhenBorrowedBookStatusIsNotBorrowed()
    {
        var borrowerId = 2;
        var bookId = 3;
        var extendDaysCount = 5;
        var borrowerAccountId = 4;
        var borrower = new Client(borrowerAccountId) { Id = borrowerId };
        var initialBorrowedUntil = DateTime.Now.AddDays(2);
        var borrowRecord = new BorrowRecord(borrowerId, 34, bookId, initialBorrowedUntil)
        {
            BorrowedBook = new Book(BookStatus.Available, 31)
            {
                Id = bookId,
                BorrowedById = borrowerId
            }
        };
        var clientRestrictions = new ClientRestrictions(
            default!,
            default!,
            default!,
            Limit.PerPeriodInMonths(10, 4),
            default!,
            default!);
        var extensionDaysForPeriod =
            clientRestrictions.ExtensionDaysLimit.ItemCount - extendDaysCount - 2;

        _clientEntityService.GetById(borrowerId).Returns(borrower);
        _borrowRecordQueryService.GetActiveBorrowRecordWithBook(borrowerId, bookId)
            .Returns(borrowRecord);
        _restrictionsService.GetRestrictionsForAccount(borrowerAccountId)
            .Returns(Result.Valid(clientRestrictions));
        _extensionQueryService
            .GetExtensionDaysForPeriod(borrowerId, Arg.Any<DateTime>(), Arg.Any<DateTime>())
            .Returns(extensionDaysForPeriod);
        _entityService.Insert(Arg.Any<Extension>(), _validator)
            .Returns(call => Result.Valid(call.Arg<Extension>()));

        var success = _extensionService.Extend(borrowerId, bookId, extendDaysCount);

        Assert.IsFalse(success);
    }

    [TestMethod]
    public void Extend_ShouldReturnFalse_WhenBorrowedBookIsNotFound()
    {
        var borrowerId = 2;
        var bookId = 3;
        var extendDaysCount = 5;
        var borrowerAccountId = 4;
        var borrower = new Client(borrowerAccountId) { Id = borrowerId };
        var initialBorrowedUntil = DateTime.Now.AddDays(2);
        var borrowRecord = new BorrowRecord(borrowerId, 34, bookId, initialBorrowedUntil);
        var clientRestrictions = new ClientRestrictions(
            default!,
            default!,
            default!,
            Limit.PerPeriodInMonths(10, 4),
            default!,
            default!);
        var extensionDaysForPeriod =
            clientRestrictions.ExtensionDaysLimit.ItemCount - extendDaysCount - 2;

        _clientEntityService.GetById(borrowerId).Returns(borrower);
        _borrowRecordQueryService.GetActiveBorrowRecordWithBook(borrowerId, bookId)
            .Returns(borrowRecord);
        _restrictionsService.GetRestrictionsForAccount(borrowerAccountId)
            .Returns(Result.Valid(clientRestrictions));
        _extensionQueryService
            .GetExtensionDaysForPeriod(borrowerId, Arg.Any<DateTime>(), Arg.Any<DateTime>())
            .Returns(extensionDaysForPeriod);
        _entityService.Insert(Arg.Any<Extension>(), _validator)
            .Returns(call => Result.Valid(call.Arg<Extension>()));

        var success = _extensionService.Extend(borrowerId, bookId, extendDaysCount);

        Assert.IsFalse(success);
    }

    [TestMethod]
    public void Extend_ShouldReturnFalse_WhenBorrowRecordIsNotFound()
    {
        var borrowerId = 2;
        var bookId = 3;
        var extendDaysCount = 5;
        var borrowerAccountId = 4;
        var borrower = new Client(borrowerAccountId) { Id = borrowerId };
        var initialBorrowedUntil = DateTime.Now.AddDays(2);
        var borrowRecord = new BorrowRecord(borrowerId, 34, bookId, initialBorrowedUntil)
        {
            BorrowedBook = new Book(BookStatus.Borrowed, 31)
            {
                Id = bookId,
                BorrowedById = borrowerId
            }
        };
        var clientRestrictions = new ClientRestrictions(
            default!,
            default!,
            default!,
            Limit.PerPeriodInMonths(10, 4),
            default!,
            default!);
        var extensionDaysForPeriod =
            clientRestrictions.ExtensionDaysLimit.ItemCount - extendDaysCount - 2;

        _clientEntityService.GetById(borrowerId).Returns(borrower);
        _borrowRecordQueryService.GetActiveBorrowRecordWithBook(borrowerId, bookId)
            .Returns((BorrowRecord?)null);
        _restrictionsService.GetRestrictionsForAccount(borrowerAccountId)
            .Returns(Result.Valid(clientRestrictions));
        _extensionQueryService
            .GetExtensionDaysForPeriod(borrowerId, Arg.Any<DateTime>(), Arg.Any<DateTime>())
            .Returns(extensionDaysForPeriod);
        _entityService.Insert(Arg.Any<Extension>(), _validator)
            .Returns(call => Result.Valid(call.Arg<Extension>()));

        var success = _extensionService.Extend(borrowerId, bookId, extendDaysCount);

        Assert.IsFalse(success);
    }

    [TestMethod]
    public void Extend_ShouldReturnFalse_WhenClientIsNotFound()
    {
        var borrowerId = 2;
        var bookId = 3;
        var extendDaysCount = 5;
        var borrowerAccountId = 4;
        var borrower = new Client(borrowerAccountId) { Id = borrowerId };
        var initialBorrowedUntil = DateTime.Now.AddDays(2);
        var borrowRecord = new BorrowRecord(borrowerId, 34, bookId, initialBorrowedUntil)
        {
            BorrowedBook = new Book(BookStatus.Borrowed, 31)
            {
                Id = bookId,
                BorrowedById = borrowerId
            }
        };
        var clientRestrictions = new ClientRestrictions(
            default!,
            default!,
            default!,
            Limit.PerPeriodInMonths(10, 4),
            default!,
            default!);
        var extensionDaysForPeriod =
            clientRestrictions.ExtensionDaysLimit.ItemCount - extendDaysCount - 2;

        _clientEntityService.GetById(borrowerId).Returns((Client?)null);
        _borrowRecordQueryService.GetActiveBorrowRecordWithBook(borrowerId, bookId)
            .Returns(borrowRecord);
        _restrictionsService.GetRestrictionsForAccount(borrowerAccountId)
            .Returns(Result.Valid(clientRestrictions));
        _extensionQueryService
            .GetExtensionDaysForPeriod(borrowerId, Arg.Any<DateTime>(), Arg.Any<DateTime>())
            .Returns(extensionDaysForPeriod);
        _entityService.Insert(Arg.Any<Extension>(), _validator)
            .Returns(call => Result.Valid(call.Arg<Extension>()));

        var success = _extensionService.Extend(borrowerId, bookId, extendDaysCount);

        Assert.IsFalse(success);
    }

    [TestMethod]
    public void Extend_ShouldReturnFalse_WhenExtendedDaysCountIsNegative()
    {
        var borrowerId = 2;
        var bookId = 3;
        var extendDaysCount = -5;
        var borrowerAccountId = 4;
        var borrower = new Client(borrowerAccountId) { Id = borrowerId };
        var initialBorrowedUntil = DateTime.Now.AddDays(2);
        var borrowRecord = new BorrowRecord(borrowerId, 34, bookId, initialBorrowedUntil)
        {
            BorrowedBook = new Book(BookStatus.Borrowed, 31)
            {
                Id = bookId,
                BorrowedById = borrowerId
            }
        };
        var clientRestrictions = new ClientRestrictions(
            default!,
            default!,
            default!,
            Limit.PerPeriodInMonths(10, 4),
            default!,
            default!);
        var extensionDaysForPeriod =
            clientRestrictions.ExtensionDaysLimit.ItemCount - extendDaysCount - 2;

        _clientEntityService.GetById(borrowerId).Returns(borrower);
        _borrowRecordQueryService.GetActiveBorrowRecordWithBook(borrowerId, bookId)
            .Returns(borrowRecord);
        _restrictionsService.GetRestrictionsForAccount(borrowerAccountId)
            .Returns(Result.Valid(clientRestrictions));
        _extensionQueryService
            .GetExtensionDaysForPeriod(borrowerId, Arg.Any<DateTime>(), Arg.Any<DateTime>())
            .Returns(extensionDaysForPeriod);
        _entityService.Insert(Arg.Any<Extension>(), _validator)
            .Returns(call => Result.Valid(call.Arg<Extension>()));

        var success = _extensionService.Extend(borrowerId, bookId, extendDaysCount);

        Assert.IsFalse(success);
    }

    [TestMethod]
    public void Extend_ShouldUpdateBorrowRecord_WhenAllValidationsPass()
    {
        var borrowerId = 2;
        var bookId = 3;
        var extendDaysCount = 5;
        var borrowerAccountId = 4;
        var borrower = new Client(borrowerAccountId) { Id = borrowerId };
        var initialBorrowedUntil = DateTime.Now.AddDays(2);
        var borrowRecord = new BorrowRecord(borrowerId, 34, bookId, initialBorrowedUntil)
        {
            BorrowedBook = new Book(BookStatus.Borrowed, 31)
            {
                Id = bookId,
                BorrowedById = borrowerId
            }
        };
        var clientRestrictions = new ClientRestrictions(
            default!,
            default!,
            default!,
            Limit.PerPeriodInMonths(10, 4),
            default!,
            default!);
        var extensionDaysForPeriod =
            clientRestrictions.ExtensionDaysLimit.ItemCount - extendDaysCount - 2;

        _clientEntityService.GetById(borrowerId).Returns(borrower);
        _borrowRecordQueryService.GetActiveBorrowRecordWithBook(borrowerId, bookId)
            .Returns(borrowRecord);
        _restrictionsService.GetRestrictionsForAccount(borrowerAccountId)
            .Returns(Result.Valid(clientRestrictions));
        _extensionQueryService
            .GetExtensionDaysForPeriod(borrowerId, Arg.Any<DateTime>(), Arg.Any<DateTime>())
            .Returns(extensionDaysForPeriod);
        _entityService.Insert(Arg.Any<Extension>(), _validator)
            .Returns(call => Result.Valid(call.Arg<Extension>()));

        var success = _extensionService.Extend(borrowerId, bookId, extendDaysCount);

        Assert.IsTrue(success);
        Assert.AreEqual(initialBorrowedUntil.AddDays(extendDaysCount), borrowRecord.BorrowedUntil);
    }

    [TestMethod]
    public void Extend_ShouldInsertCorrectExtension_WhenAllValidationsPass()
    {
        Extension? extension = null;
        var borrowerId = 2;
        var bookId = 3;
        var extendDaysCount = 5;
        var borrowerAccountId = 4;
        var borrower = new Client(borrowerAccountId) { Id = borrowerId };
        var initialBorrowedUntil = DateTime.Now.AddDays(2);
        var borrowRecord = new BorrowRecord(borrowerId, 34, bookId, initialBorrowedUntil)
        {
            BorrowedBook = new Book(BookStatus.Borrowed, 31)
            {
                Id = bookId,
                BorrowedById = borrowerId
            }
        };
        var clientRestrictions = new ClientRestrictions(
            default!,
            default!,
            default!,
            Limit.PerPeriodInMonths(10, 4),
            default!,
            default!);
        var extensionDaysForPeriod =
            clientRestrictions.ExtensionDaysLimit.ItemCount - extendDaysCount - 2;

        _clientEntityService.GetById(borrowerId).Returns(borrower);
        _borrowRecordQueryService.GetActiveBorrowRecordWithBook(borrowerId, bookId)
            .Returns(borrowRecord);
        _restrictionsService.GetRestrictionsForAccount(borrowerAccountId)
            .Returns(Result.Valid(clientRestrictions));
        _extensionQueryService
            .GetExtensionDaysForPeriod(borrowerId, Arg.Any<DateTime>(), Arg.Any<DateTime>())
            .Returns(extensionDaysForPeriod);
        _entityService.Insert(Arg.Do<Extension>(x => extension = x), _validator)
            .Returns(call => Result.Valid(call.Arg<Extension>()));

        var success = _extensionService.Extend(borrowerId, bookId, extendDaysCount);

        Assert.IsTrue(success);
        Assert.IsNotNull(extension);
        Assert.AreEqual(extendDaysCount, extension.DayCount);
        Assert.AreEqual(borrowerId, extension.RequestedById);
        var timeSpan = DateTime.Now - extension.CreatedDateTime;
        Assert.IsTrue(timeSpan.TotalMinutes < 1);
    }

    [TestMethod]
    public void Extend_ShouldReturnTrue_WhenAllValidationsPass()
    {
        var borrowerId = 2;
        var bookId = 3;
        var extendDaysCount = 5;
        var borrowerAccountId = 4;
        var borrower = new Client(borrowerAccountId) { Id = borrowerId };
        var initialBorrowedUntil = DateTime.Now.AddDays(2);
        var borrowRecord = new BorrowRecord(borrowerId, 34, bookId, initialBorrowedUntil)
        {
            BorrowedBook = new Book(BookStatus.Borrowed, 31)
            {
                Id = bookId ,
                BorrowedById = borrowerId
            }
        };
        var clientRestrictions = new ClientRestrictions(
            default!,
            default!,
            default!,
            Limit.PerPeriodInMonths(10, 4),
            default!,
            default!);
        var extensionDaysForPeriod =
            clientRestrictions.ExtensionDaysLimit.ItemCount - extendDaysCount - 2;

        _clientEntityService.GetById(borrowerId).Returns(borrower);
        _borrowRecordQueryService.GetActiveBorrowRecordWithBook(borrowerId, bookId)
            .Returns(borrowRecord);
        _restrictionsService.GetRestrictionsForAccount(borrowerAccountId)
            .Returns(Result.Valid(clientRestrictions));
        _extensionQueryService
            .GetExtensionDaysForPeriod(borrowerId, Arg.Any<DateTime>(), Arg.Any<DateTime>())
            .Returns(extensionDaysForPeriod);
        _entityService.Insert(Arg.Any<Extension>(), _validator)
            .Returns(call => Result.Valid(call.Arg<Extension>()));

        var success = _extensionService.Extend(borrowerId, bookId, extendDaysCount);

        Assert.IsTrue(success);
    }
}