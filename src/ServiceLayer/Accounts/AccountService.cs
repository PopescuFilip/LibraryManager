using DomainModel;
using FluentValidation;
using ServiceLayer.CRUD;

namespace ServiceLayer.Accounts;

public interface IAccountService
{
    Result<Account> Create(string name, string address, string? email, string? phoneNumber);
    Result<Account> Update(int accountId, AccountUpdateOptions options);
    Result<Client> CreateClient(int accountId);
    Result<Employee> CreateEmployee(int accountId);
}

public class AccountService(
    IEntityService<Account> _entityService,
    IEntityService<Client> _clientEntityService,
    IEntityService<Employee> _employeeEntityService,
    IValidator<Account> _validator)
    : IAccountService
{
    public Result<Account> Create(string name, string address, string? email, string? phoneNumber)
    {
        var account = new Account(name, address, email, phoneNumber);
        return _entityService.Insert(account, _validator);
    }

    public Result<Account> Update(int accountId, AccountUpdateOptions options)
    {
        var account = _entityService.GetById(accountId);

        if (account is null)
            return Result.Invalid();

        options.ApplyTo(account);
        return _entityService.Update(account, _validator);
    }

    public Result<Client> CreateClient(int accountId)
    {
        var account = _entityService.GetById(accountId);

        if (account is null)
            return Result.Invalid();

        return new InvalidResult();
    }

    public Result<Employee> CreateEmployee(int accountId)
    {
        throw new NotImplementedException();
    }
}