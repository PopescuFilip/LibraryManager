using DataMapper;
using DataMapper.QueryHelpers;
using DomainModel;
using System.Linq.Expressions;

namespace ServiceLayer.CRUD;

public interface IBorrowRecordQueryService
{
    int GetBooksLendedTodayCount(int employeeId);
    int GetBooksBorrowedInPeriodCount(int clientId, DateTime start, DateTime end);
}

public class BorrowRecordQueryService(IRepository<BorrowRecord> _repository)
    : IBorrowRecordQueryService
{
    public int GetBooksLendedTodayCount(int employeeId)
    {
        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);

        Expression<Func<BorrowRecord, bool>> filter = x =>
            x.LenderId == employeeId
            && today <= x.BorrowDateTime
            && x.BorrowDateTime < tomorrow;

        return _repository.Get(
            Select<BorrowRecord>.Id,
            Collector<int>.Count,
            asNoTracking: true,
            Order<BorrowRecord>.ById,
            filter
            );
    }

    public int GetBooksBorrowedInPeriodCount(int clientId, DateTime start, DateTime end)
    {
        Expression<Func<BorrowRecord, bool>> filter = x =>
            x.BorrowerId == clientId
            && start <= x.BorrowDateTime
            && x.BorrowDateTime <= end;

        return _repository.Get(
            Select<BorrowRecord>.Id,
            Collector<int>.Count,
            asNoTracking: true,
            Order<BorrowRecord>.ById,
            filter);
    }
}