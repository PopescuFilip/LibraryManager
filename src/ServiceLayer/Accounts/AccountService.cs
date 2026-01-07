using DomainModel;
using ServiceLayer.Authors;
using ServiceLayer.CRUD;

namespace ServiceLayer.Accounts;

public interface IAccountService
{
    Result<Account> Create(AuthorOptions options);
    Result<Account> Update(AuthorOptions options);
}

public class AccountService(IEntityService<Account> _entityService)
{
}