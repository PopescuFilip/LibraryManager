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
            return Result.Valid(_clientRestrictionsProvider.GetPrivilegedClientRestrictions());

        return Result.Valid(_clientRestrictionsProvider.GetClientRestrictions());
    }
}