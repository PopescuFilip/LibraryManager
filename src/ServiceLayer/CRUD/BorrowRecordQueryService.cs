using DataMapper;
using DataMapper.QueryHelpers;
using DomainModel;
using System.Linq.Expressions;

namespace ServiceLayer.CRUD;

public interface IBorrowRecordQueryService
{
    int GetBooksLendedTodayCount(int employeeId);
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
            x => x.BorrowedBooks.Count,
            Collector<int>.ToList,
            asNoTracking: true,
            Order<BorrowRecord>.ById,
            filter,
            includeProperties: x => x.BorrowedBooks
            ).Sum();
    }
}