using DomainModel;
using ServiceLayer.CRUD;

namespace ServiceLayer.Accounts;

public class AccountService(IEntityService<Account> _entityService)
{
}