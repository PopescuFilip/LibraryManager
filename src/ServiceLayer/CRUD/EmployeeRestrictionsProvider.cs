using DataMapper;
using DomainModel.Restrictions;

namespace ServiceLayer.CRUD;

public interface IEmployeeRestrictionsProvider
{
    EmployeeRestrictions Get();
}

public class EmployeeRestrictionsProvider(IRestrictionsProvider _restrictionsProvider)
    : IEmployeeRestrictionsProvider
{
    public EmployeeRestrictions Get() => _restrictionsProvider
        .GetRestrictions()!
        .ToEmployeeRestrictions();
}