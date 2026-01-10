using DomainModel;
using DomainModel.Restrictions;
using FluentValidation;
using ServiceLayer.CRUD;
using ServiceLayer.Restriction;

namespace ServiceLayer.Borrowing;

public interface IBorrowService
{
    bool Borrow(int borrowerId, int lenderId, IdCollection bookIds);
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

    public bool Borrow(int borrowerId, int lenderId, IdCollection bookIds)
    {
        if (!_idCollectionValidator.Validate(bookIds).IsValid)
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

        var booksLendedToday = _borrowRecordQueryService.GetBooksLendedTodayCount(lender.Id);

        var someBooks = new List<Book>();
        var borrowRecord = new BorrowRecord(borrowerId, lenderId, someBooks);
        return _entityService.Insert(borrowRecord, _validator).IsValid;
    }
}