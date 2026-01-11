using DomainModel;
using DomainModel.DTOs;
using DomainModel.Restrictions;
using FluentValidation;
using ServiceLayer.CRUD;
using ServiceLayer.Restriction;
using System.Collections.Immutable;

namespace ServiceLayer.Borrowing;

public interface IBorrowService
{
    bool Borrow(int borrowerId, int lenderId, ImmutableArray<BorrowOptions> options);

    bool BorrowNoValidation(int borrowerId, int lenderId, ImmutableArray<BorrowOptions> options);
}

public class BorrowService(
    IEntityService<BorrowRecord> _entityService,
    IEntityService<Client> _clientEntityService,
    IEntityService<Employee> _employeeEntityService,
    IValidator<IdCollection> _idCollectionValidator,
    IRestrictionsService _restrictionsService,
    IEmployeeRestrictionsProvider _employeeRestrictionsProvider,
    IBorrowRecordQueryService _borrowRecordQueryService,
    IBookQueryService _bookQueryService,
    IDomainQueryService _domainQueryService
    )
    : IBorrowService
{
    private const int MultipleDomainRequirementThreshold = 3;

    private readonly IValidator<BorrowRecord> _validator = EmptyValidator.Create<BorrowRecord>();

    public bool Borrow(int borrowerId, int lenderId, ImmutableArray<BorrowOptions> options)
    {
        var bookIds = options.ToIdCollection();
        if (!_idCollectionValidator.Validate(bookIds).IsValid)
            return false;

        if (options.Any(x => x.BorrowUntil <= DateTime.Now))
            return false;

        var borrower = _clientEntityService.GetById(borrowerId);
        if (borrower is null)
            return false;

        var lender = _employeeEntityService.GetById(lenderId);
        if (lender is null)
            return false;

        var clientRestrictionsResult = _restrictionsService.GetRestrictionsForAccount(borrower.AccountId);
        if (!clientRestrictionsResult.IsValid)
            return false;

        var clientRestrictions = clientRestrictionsResult.Get();
        if (clientRestrictions.BorrowedBooksPerRequestLimit.ExceedsLimit(bookIds.Count))
            return false;

        var employeeRestrictionsResult = _employeeRestrictionsProvider.Get();
        if (!employeeRestrictionsResult.IsValid)
            return false;

        var employeeRestrictions = employeeRestrictionsResult.Get();
        var booksLendedToday = _borrowRecordQueryService.GetBooksLendedTodayCount(lender.Id);
        var booksLendedAfterBorrow = booksLendedToday + bookIds.Count;
        if (employeeRestrictions.BorrowedBooksGivenLimit.ExceedsLimit(booksLendedAfterBorrow))
            return false;

        var booksBorrowedToday = _borrowRecordQueryService.GetBooksBorrowedTodayCount(borrower.Id);
        var booksBorrowedAfterBorrow = booksBorrowedToday + bookIds.Count;
        if (clientRestrictions.BorrowedBooksPerDayLimit.ExceedsLimit(booksBorrowedAfterBorrow))
            return false;

        var startTime = clientRestrictions.BorrowedBooksLimit.GetStartTimeToCheck();
        var booksBorrowed = _borrowRecordQueryService.GetBooksBorrowedInPeriodCount(borrower.Id, startTime, DateTime.Now);
        if (clientRestrictions.BorrowedBooksLimit.ExceedsLimit(booksBorrowed + bookIds.Count))
            return false;

        var bookDetails = _bookQueryService.GetBookDetails(bookIds);
        if (bookDetails.Count != bookIds.Count ||
            bookDetails.ContainsDuplicateBookEdition() ||
            bookDetails.Any(x => x.Book.Status != BookStatus.Available) ||
            bookDetails.Any(x => x.BooksAvailable / x.BooksTotal < 0.1))
            return false;

        var sameBookStart = clientRestrictions.BorrowedSameBookLimit.GetStartTimeToCheck();
        foreach (var item in bookDetails)
        {
            var booksInPeriod = _borrowRecordQueryService.GetBooksBorrowedInPeriodCount(
                borrower.Id, item.BookEditionId, sameBookStart, DateTime.Now);

            if (clientRestrictions.BorrowedSameBookLimit.ExceedsLimit(booksInPeriod + 1))
                return false;
        }

        if (bookIds.Count >= MultipleDomainRequirementThreshold)
        {
            var distinctParentDomainsCount = _domainQueryService
                .GetParentIds(bookDetails.SelectDomainIds())
                .Distinct()
                .Count();

            if (distinctParentDomainsCount < 2)
                return false;
        }

        if (!ValidateSameDomainLimit(borrower, bookDetails, clientRestrictions))
            return false;

        var borrowRecords = options
            .Select(options => new BorrowRecord(borrower.Id, lender.Id, options.BookId, options.BorrowUntil))
            .ToImmutableArray();
        foreach (var book in bookDetails.Select(x => x.Book))
        {
            book.Status = BookStatus.Borrowed;
            book.BorrowedById = borrower.Id;
        }
        return _entityService.InsertRange(borrowRecords, _validator);
    }

    public bool BorrowNoValidation(int borrowerId, int lenderId, ImmutableArray<BorrowOptions> options)
    {
        var borrowRecords = options
            .Select(options => new BorrowRecord(borrowerId, lenderId, options.BookId, options.BorrowUntil))
            .ToImmutableArray();
        var bookDetails = _bookQueryService.GetBookDetails(options.ToIdCollection());
        foreach (var book in bookDetails.Select(x => x.Book))
        {
            book.Status = BookStatus.Borrowed;
            book.BorrowedById = borrowerId;
        }
        return _entityService.InsertRange(borrowRecords, _validator);
    }

    private bool ValidateSameDomainLimit(Client borrower, IReadOnlyCollection<BookDetails> bookDetails, ClientRestrictions clientRestrictions)
    {
        var sameDomainStartTime = clientRestrictions.SameDomainBorrowedBooksLimit.GetStartTimeToCheck();

        var borrowedBookDomainIds = _borrowRecordQueryService
            .GetDomainIdsForBorrowedInPeriod(borrower.Id, sameDomainStartTime, DateTime.Now)
            .Select(x => new BookDomainIds(x));
        var currentBookDomainIds = bookDetails.Select(x => new BookDomainIds(x.DomainIds));
        var allDomainIds = borrowedBookDomainIds.Concat(currentBookDomainIds);

        var parentDomainIds = _domainQueryService.GetParentDomainIds(allDomainIds);

        var maxParentDomain = parentDomainIds.SelectMany(bookParentDomainIds => bookParentDomainIds.Select(x => x))
            .GroupBy(x => x)
            .Max(x => x.Count());

        if (clientRestrictions.SameDomainBorrowedBooksLimit.ExceedsLimit(maxParentDomain))
            return false;

        return true;
    }
}