using DomainModel.Restrictions;
using ServiceLayer.CRUD;

namespace ServiceLayer.Restriction;

public interface IRestrictionsService
{
    Result<ClientRestrictions> GetRestrictionsForAccount(int accountId);
}

public class RestrictionsService(
    IClientRestrictionsProvider _clientRestrictionsProvider,
    IAccountQueryService _accountQueryService)
    : IRestrictionsService
{
    public Result<ClientRestrictions> GetRestrictionsForAccount(int accountId)
    {
        if (!_accountQueryService.ClientForAccountExists(accountId))
            return Result.Invalid();

        if (_accountQueryService.EmployeeForAccountExists(accountId))
            return _clientRestrictionsProvider.GetPrivilegedClientRestrictions();

        return _clientRestrictionsProvider.GetClientRestrictions();
    }
}