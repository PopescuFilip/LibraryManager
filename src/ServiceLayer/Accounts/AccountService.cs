using DomainModel;
using FluentValidation;
using ServiceLayer.CRUD;

namespace ServiceLayer.Accounts;

public interface IAccountService
{
    Result<Account> Create(AccountOptions options);
    Result<Account> Update(AccountOptions options);
}

public class AccountService(
    IEntityService<Account> _entityService,
    IValidator<AccountOptions> _optionsValidator)
    : IAccountService
{
    public Result<Account> Create(AccountOptions options)
    {
        throw new NotImplementedException();
    }

    public Result<Account> Update(AccountOptions options)
    {
        throw new NotImplementedException();
    }
}