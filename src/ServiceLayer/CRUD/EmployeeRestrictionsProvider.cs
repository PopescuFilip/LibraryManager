using DataMapper;
using DomainModel.Restrictions;

namespace ServiceLayer.CRUD;

public interface IEmployeeRestrictionsProvider
{
    Result<EmployeeRestrictions> Get();
}

public class EmployeeRestrictionsProvider(IRestrictionsProvider _restrictionsProvider)
    : IEmployeeRestrictionsProvider
{
    public Result<EmployeeRestrictions> Get()
    {
        var restrictions = _restrictionsProvider.GetRestrictions();
        if (restrictions is null)
            return Result.Invalid();

        return Result.Valid(restrictions.ToEmployeeRestrictions());
    }
}