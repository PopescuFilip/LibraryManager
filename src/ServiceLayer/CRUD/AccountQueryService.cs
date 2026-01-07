using DataMapper;
using DomainModel;

namespace ServiceLayer.CRUD;

public interface IAccountQueryService
{
    bool ClientForAccountExists(int accountId);
    bool EmployeeForAccountExists(int accountId);
}

public class AccountQueryService(
    IRepository<Client> _clientRepository,
    IRepository<Employee> _employeeRepository)
    : IAccountQueryService
{
    public bool ClientForAccountExists(int accountId) =>
        _clientRepository.Exists(x => x.AccountId == accountId);

    public bool EmployeeForAccountExists(int accountId) =>
        _employeeRepository.Exists(x => x.AccountId == accountId);
}