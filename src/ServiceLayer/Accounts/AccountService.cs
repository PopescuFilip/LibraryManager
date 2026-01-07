using DomainModel;
using FluentValidation;
using ServiceLayer.CRUD;

namespace ServiceLayer.Accounts;

public interface IAccountService
{
    Result<Account> Create(string name, string address, string? email, string? phoneNumber);
    Result<Account> Update(AccountOptions options);
}

public class AccountService(
    IEntityService<Account> _entityService,
    IValidator<Account> _validator)
    : IAccountService
{
    public Result<Account> Create(string name, string address, string? email, string? phoneNumber)
    {
        var account = new Account(name, address, email, phoneNumber);
        return _entityService.Insert(account, _validator);
    }

    public Result<Account> Update(AccountOptions options)
    {
        throw new NotImplementedException();
    }
}