using DomainModel;
using FluentValidation;
using ServiceLayer.CRUD;

namespace ServiceLayer.Accounts;

public interface IAccountService
{
    Result<Account> Create(string name, string address, string? email, string? phoneNumber);
    Result<Account> Update(int id, AccountUpdateOptions options);
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

    public Result<Account> Update(int id, AccountUpdateOptions options)
    {
        var account = _entityService.GetById(id);

        if (account is null)
            return Result.Invalid();

        options.ApplyTo(account);
        return _entityService.Update(account, _validator);
    }
}