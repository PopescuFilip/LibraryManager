using DomainModel;
using DomainModel.Restrictions;
using FluentValidation;
using ServiceLayer.CRUD;
using ServiceLayer.Restriction;
using System.Collections.Immutable;

namespace ServiceLayer.Borrowing;

public interface IBorrowService
{
    bool Borrow(int borrowerId, int lenderId, ImmutableArray<BorrowOptions> options);
}

public class BorrowService(
    IEntityService<BorrowRecord> _entityService,
    IEntityService<Client> _clientEntityService,
    IEntityService<Employee> _employeeEntityService,
    IValidator<IdCollection> _idCollectionValidator,
    IRestrictionsService _restrictionsService,
    IEmployeeRestrictionsProvider _employeeRestrictionsProvider,
    IBorrowRecordQueryService _borrowRecordQueryService
    )
    : IBorrowService
{
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

        var borrowRecord = new BorrowRecord(borrowerId, lenderId, 1, DateTime.Now);
        return _entityService.Insert(borrowRecord, _validator).IsValid;
    }
}