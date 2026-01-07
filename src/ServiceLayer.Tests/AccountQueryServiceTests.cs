using DomainModel;
using ServiceLayer.CRUD;
using ServiceLayer.UnitTests.TestHelpers;
using System.Diagnostics.CodeAnalysis;

namespace ServiceLayer.UnitTests;

[TestClass]
[ExcludeFromCodeCoverage]
public class AccountQueryServiceTests
{
    private AccountQueryService _queryService = default!;
    private FakeRepository<Client> _clientRepository = default!;
    private FakeRepository<Employee> _employeeRepository = default!;

    [TestInitialize]
    public void Init()
    {
        _clientRepository = new FakeRepository<Client>();
        _employeeRepository = new FakeRepository<Employee>();
        _queryService = new AccountQueryService(_clientRepository, _employeeRepository);
    }

    [TestMethod]
    public void ClientForAccountExists_ShouldReturnFalse_WhenClientDoesNotExist()
    {
        var interestingAccountId = 214;
        var idsWithAccountIds = new List<(int Id, int AccountId)>()
        {
            (1, 21),
            (2, 22),
            (3, 30),
            (4, 31),
            (6, 131),
            (7, 311),
        };
        var clients = Generator.GenerateClientsFrom(idsWithAccountIds);
        _clientRepository.SetSourceValues(clients);

        var exists = _queryService.ClientForAccountExists(interestingAccountId);

        Assert.IsFalse(exists);
    }

    [TestMethod]
    public void ClientForAccountExists_ShouldReturnTrue_WhenClientExists()
    {
        var interestingAccountId = 24;
        var idsWithAccountIds = new List<(int Id, int AccountId)>()
        {
            (1, 21),
            (2, 22),
            (3, interestingAccountId),
            (4, 31),
            (6, 131),
            (7, 311),
        };
        var clients = Generator.GenerateClientsFrom(idsWithAccountIds);
        _clientRepository.SetSourceValues(clients);

        var exists = _queryService.ClientForAccountExists(interestingAccountId);

        Assert.IsTrue(exists);
    }

    [TestMethod]
    public void EmployeeForAccountExists_ShouldReturnFalse_WhenEmployeeDoesNotExist()
    {
        var interestingAccountId = 214;
        var idsWithAccountIds = new List<(int Id, int AccountId)>()
        {
            (1, 21),
            (2, 22),
            (3, 30),
            (4, 31),
            (6, 131),
            (7, 311),
        };
        var employees = Generator.GenerateEmployeesFrom(idsWithAccountIds);
        _employeeRepository.SetSourceValues(employees);

        var exists = _queryService.EmployeeForAccountExists(interestingAccountId);

        Assert.IsFalse(exists);
    }

    [TestMethod]
    public void EmployeeForAccountExists_ShouldReturnTrue_WhenEmployeeExists()
    {
        var interestingAccountId = 24;
        var idsWithAccountIds = new List<(int Id, int AccountId)>()
        {
            (1, 21),
            (2, 22),
            (3, interestingAccountId),
            (4, 31),
            (6, 131),
            (7, 311),
        };
        var employees = Generator.GenerateEmployeesFrom(idsWithAccountIds);
        _employeeRepository.SetSourceValues(employees);

        var exists = _queryService.EmployeeForAccountExists(interestingAccountId);

        Assert.IsTrue(exists);
    }
}