using DomainModel;
using DomainModel.Restrictions;
using FluentValidation;
using ServiceLayer.CRUD;
using ServiceLayer.Restriction;

namespace ServiceLayer.Borrowing;

public interface IExtensionService
{
    bool Extend(int borrowerId, int bookId, int extendByDayCount);
}

public class ExtensionService(
    IEntityService<Extension> _entityService,
    IEntityService<Client> _clientEntityService,
    IRestrictionsService _restrictionsService,
    IExtensionQueryService _extensionQueryService,
    IBorrowRecordQueryService _borrowRecordQueryService)
    : IExtensionService
{
    private readonly IValidator<Extension> _validator = EmptyValidator.Create<Extension>();

    public bool Extend(int borrowerId, int bookId, int extendByDayCount)
    {
        var borrower = _clientEntityService.GetById(borrowerId);
        if (borrower is null)
            return false;

        var borrowRecord = _borrowRecordQueryService.GetActiveBorrowRecordWithBook(borrower.Id, bookId);
        if (borrowRecord is null)
            return false;

        if (borrowRecord.BorrowedBook is null ||
            borrowRecord.BorrowedBook.Status != BookStatus.Borrowed ||
            borrowRecord.BorrowedBook.BorrowedById != borrower.Id)
            return false;

        var result = _restrictionsService.GetRestrictionsForAccount(borrower.AccountId);
        if (!result.IsValid)
            return false;
        var restrictions = result.Get();

        var start = restrictions.ExtensionDaysLimit.GetStartTimeToCheck();
        var extensionDaysUntilNow = _extensionQueryService
            .GetExtensionDaysForPeriod(borrowerId, start, DateTime.Now);
        var totalExtensionDays = extensionDaysUntilNow + extendByDayCount;
        if (restrictions.ExtensionDaysLimit.ExceedsLimit(totalExtensionDays))
            return false;

        borrowRecord.BorrowedUntil = borrowRecord.BorrowedUntil.AddDays(extendByDayCount);
        var extension = new Extension(borrower.Id, extendByDayCount);
        return _entityService.Insert(extension, _validator).IsValid;
    }
}