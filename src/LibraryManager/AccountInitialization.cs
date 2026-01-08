using DomainModel;
using Microsoft.Extensions.DependencyInjection;
using ServiceLayer.Accounts;
using SimpleInjector;
using System.Diagnostics.CodeAnalysis;

namespace LibraryManager;

[ExcludeFromCodeCoverage]
public static class AccountInitialization
{
    public static void InitAccounts(this Scope scope)
    {
        if (scope.GetAllEntities<Account>().Count != 0)
            return;

        var accountService = scope.GetRequiredService<IAccountService>();

        var account = accountService.Create("client", "Maple", "client@gmail.com", null);
        _ = accountService.CreateClient(account.Get().Id);

        var employeeAccount = accountService.Create("employee", "Sth", "employee@gmail.com", null);
        _ = accountService.CreateClient(employeeAccount.Get().Id);
        _ = accountService.CreateEmployee(employeeAccount.Get().Id);
    }
}