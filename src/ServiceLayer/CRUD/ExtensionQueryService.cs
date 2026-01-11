using DataMapper;
using DataMapper.QueryHelpers;
using DomainModel;
using System.Linq.Expressions;

namespace ServiceLayer.CRUD;

public interface IExtensionQueryService
{
    int GetExtensionDaysForPeriod(int clientId, DateTime start, DateTime end);
}

public class ExtensionQueryService(IRepository<Extension> _repository)
    : IExtensionQueryService
{
    public int GetExtensionDaysForPeriod(int clientId, DateTime start, DateTime end)
    {
        Expression<Func<Extension, bool>> filter = x =>
            x.RequestedById == clientId
            && start <= x.CreatedDateTime
            && x.CreatedDateTime <= end;

        return _repository.Get(
            x => x.DayCount,
            Collector<int>.ToList,
            asNoTracking: true,
            Order<Extension>.ById)
            .Sum();
    }
}