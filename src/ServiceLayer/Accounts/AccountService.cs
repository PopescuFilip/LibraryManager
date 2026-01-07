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
    IValidator<Account> _validator,
    IAccountQueryService _queryService)
    : IAccountService
{
    private readonly IValidator<Client> _clientValidator = EmptyValidator.Create<Client>();
    private readonly IValidator<Employee> _employeeValidator = EmptyValidator.Create<Employee>();

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

        if (_queryService.ClientForAccountExists(accountId))
            return Result.Invalid();

        var client = new Client(accountId);
        return _clientEntityService.Insert(client, _clientValidator);
    }

    public Result<Employee> CreateEmployee(int accountId)
    {
        var account = _entityService.GetById(accountId);

        if (account is null)
            return Result.Invalid();

        if (_queryService.EmployeeForAccountExists(accountId))
            return Result.Invalid();

        var employee = new Employee(accountId);
        return _employeeEntityService.Insert(employee, _employeeValidator);
    }
}