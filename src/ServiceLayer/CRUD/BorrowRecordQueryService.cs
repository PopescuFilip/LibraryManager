using DataMapper;
using DataMapper.QueryHelpers;
using DomainModel;
using System.Collections.Immutable;
using System.Linq.Expressions;

namespace ServiceLayer.CRUD;

public interface IBorrowRecordQueryService
{
    int GetBooksLendedTodayCount(int employeeId);
    int GetBooksBorrowedTodayCount(int clientId);
    int GetBooksBorrowedInPeriodCount(int clientId, DateTime start, DateTime end);
    int GetBooksBorrowedInPeriodCount(int clientId, int bookEditionId, DateTime start, DateTime end);
    IReadOnlyCollection<ImmutableArray<int>> GetDomainIdsForBorrowedInPeriod(int clientId, DateTime start, DateTime end);
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

    public int GetBooksBorrowedTodayCount(int clientId)
    {
        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);

        Expression<Func<BorrowRecord, bool>> filter = x =>
            x.BorrowerId == clientId
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
        var filter = GetFilter(clientId, start, end);

        return _repository.Get(
            Select<BorrowRecord>.Id,
            Collector<int>.Count,
            asNoTracking: true,
            Order<BorrowRecord>.ById,
            filter);
    }

    public int GetBooksBorrowedInPeriodCount(int clientId, int bookEditionId, DateTime start, DateTime end)
    {
        Expression<Func<BorrowRecord, bool>> filter = x =>
            x.BorrowerId == clientId
            && x.BorrowedBook.BookEditionId == bookEditionId
            && start <= x.BorrowDateTime
            && x.BorrowDateTime <= end;

        return _repository.Get(
            Select<BorrowRecord>.Id,
            Collector<int>.Count,
            asNoTracking: true,
            Order<BorrowRecord>.ById,
            filter);
    }

    public IReadOnlyCollection<ImmutableArray<int>> GetDomainIdsForBorrowedInPeriod(int clientId, DateTime start, DateTime end)
    {
        var filter = GetFilter(clientId, start, end);

        return _repository.Get(
            x => x.BorrowedBook.BookEdition.BookDefinition.Domains.Select(x => x.Id).ToImmutableArray(),
            Collector<ImmutableArray<int>>.ToList,
            asNoTracking: true,
            Order<BorrowRecord>.ById,
            filter);
    }

    private static Expression<Func<BorrowRecord, bool>> GetFilter(int clientId, DateTime start, DateTime end) =>
        x =>
        x.BorrowerId == clientId
        && start <= x.BorrowDateTime
        && x.BorrowDateTime <= end;
}