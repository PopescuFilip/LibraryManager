using DomainModel;
using FluentValidation;
using NSubstitute;
using ServiceLayer.Accounts;
using ServiceLayer.CRUD;
using System.Diagnostics.CodeAnalysis;

namespace ServiceLayer.UnitTests;

[TestClass]
[ExcludeFromCodeCoverage]
public class AccountServiceTests
{
    private AccountService _accountService = default!;
    private IEntityService<Account> _entityService = default!;
    private IValidator<Account> _validator = default!;

    [TestInitialize]
    public void Init()
    {
        _entityService = Substitute.For<IEntityService<Account>>();
        _validator = Substitute.For<IValidator<Account>>();
        _accountService = new AccountService(_entityService, _validator);
    }

    [TestMethod]
    public void Create_ShouldReturnInvalid_WhenInsertFails()
    {
        var name = "namee";
        var address = "Maple Stree 189";
        string? email = "someTest@gmail.com";
        string? phoneNumber = null;
        _entityService.Insert(Arg.Any<Account>(), _validator)
            .Returns(Result.Invalid());

        var result = _accountService.Create(name, address, email, phoneNumber);

        Assert.IsFalse(result.IsValid);
    }

    [TestMethod]
    public void Create_ShouldInsertAndReturnAccount_WhenAllValidationsPass()
    {
        var name = "namee";
        var address = "Maple Stree 189";
        string? email = "someTest@gmail.com";
        string? phoneNumber = "0743123321";
        _entityService.Insert(Arg.Any<Account>(), _validator)
            .Returns(call => Result.Valid(call.Arg<Account>()));

        var result = _accountService.Create(name, address, email, phoneNumber);

        Assert.IsTrue(result.IsValid);
        var account = result.Get();
        Assert.AreEqual(name, account.Name);
        Assert.AreEqual(address, account.Address);
        Assert.AreEqual(email, account.Email);
        Assert.AreEqual(phoneNumber, account.PhoneNumber);
    }

    [TestMethod]
    public void Create_ShouldReturnValid_WhenAllValidationsPass()
    {
        var name = "namee";
        var address = "Maple Stree 189";
        string? email = "someTest@gmail.com";
        string? phoneNumber = null;
        _entityService.Insert(Arg.Any<Account>(), _validator)
            .Returns(call => Result.Valid(call.Arg<Account>()));

        var result = _accountService.Create(name, address, email, phoneNumber);

        Assert.IsTrue(result.IsValid);
    }

    [TestMethod]
    public void Update_ShouldReturnInvalid_WhenAccountWithIdIsNotFound()
    {
        var id = 123;
        var options = AccountUpdateOptions.Default;
        _entityService.GetById(id).Returns((Account?)null);
        _entityService.Update(Arg.Any<Account>(), _validator)
            .Returns(Result.Invalid());

        var result = _accountService.Update(id, options);

        Assert.IsFalse(result.IsValid);
    }

    [TestMethod]
    public void Update_ShouldReturnUpdatedAccount_WhenAllValidationsPass()
    {
        var id = 123;
        var name = "namee";
        var address = "Maple Stree 189";
        string? email = "someTest@gmail.com";
        string? phoneNumber = "0743123321";
        var account = new Account(name, address, email, phoneNumber) { Id = id };
        var newName = "newName";
        var newEmail = "someOtherTest@gmail.com";
        var options = AccountUpdateOptions.Default.WithName(newName).WithEmail(newEmail);
        _entityService.GetById(id).Returns(account);
        _entityService.Update(account, _validator)
            .Returns(Result.Valid(account));

        var result = _accountService.Update(id, options);

        Assert.IsTrue(result.IsValid);
        var updatedAccount = result.Get();
        Assert.AreEqual(newName, updatedAccount.Name);
        Assert.AreEqual(address, updatedAccount.Address);
        Assert.AreEqual(newEmail, updatedAccount.Email);
        Assert.AreEqual(phoneNumber, updatedAccount.PhoneNumber);
    }

    [TestMethod]
    public void Update_ShouldReturnValid_WhenAllValidationsPass()
    {
        var id = 123;
        var name = "namee";
        var address = "Maple Stree 189";
        string? email = "someTest@gmail.com";
        string? phoneNumber = null;
        var account = new Account(name, address, email, phoneNumber) { Id = id };
        var options = AccountUpdateOptions.Default;
        _entityService.GetById(id).Returns(account);
        _entityService.Update(account, _validator)
            .Returns(Result.Valid(account));

        var result = _accountService.Update(id, options);

        Assert.IsTrue(result.IsValid);
    }
}