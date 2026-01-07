using DomainModel;
using ServiceLayer.Accounts;
using System.Diagnostics.CodeAnalysis;

namespace ServiceLayer.UnitTests;

[TestClass]
[ExcludeFromCodeCoverage]
public class AccountUpdateOptionsExtensionsTests
{
    [TestMethod]
    public void ApplyTo_ShouldChangeOnlyPhoneNumber_WhenOnlyPhoneNumberHasChanges()
    {
        var name = "namee";
        var address = "Maple Street no. 12";
        string? email = "test@gmail.com";
        string? phoneNumber = "0765123123";
        var account = new Account(name, address, email, phoneNumber);
        string? newPhoneNumber = "+40765123123";
        var options = AccountUpdateOptions.Default.WithPhoneNumber(newPhoneNumber);

        options.ApplyTo(account);

        Assert.AreEqual(name, account.Name);
        Assert.AreEqual(address, account.Address);
        Assert.AreEqual(email, account.Email);
        Assert.AreEqual(newPhoneNumber, account.PhoneNumber);
    }

    [TestMethod]
    public void ApplyTo_ShouldChangeOnlyEmail_WhenOnlyEmailHasChanges()
    {
        var name = "namee";
        var address = "Maple Street no. 12";
        string? email = "test@gmail.com";
        string? phoneNumber = "0765123123";
        var account = new Account(name, address, email, phoneNumber);
        string? newEmail = "otherTest@gmail.com";
        var options = AccountUpdateOptions.Default.WithEmail(newEmail);

        options.ApplyTo(account);

        Assert.AreEqual(name, account.Name);
        Assert.AreEqual(address, account.Address);
        Assert.AreEqual(newEmail, account.Email);
        Assert.AreEqual(phoneNumber, account.PhoneNumber);
    }

    [TestMethod]
    public void ApplyTo_ShouldChangeOnlyAddress_WhenOnlyAddressHasChanges()
    {
        var name = "namee";
        var address = "Maple Street no. 12";
        string? email = "test@gmail.com";
        string? phoneNumber = "0765123123";
        var account = new Account(name, address, email, phoneNumber);
        var newAddress = "SomeOtherMaple Street no. 122";
        var options = AccountUpdateOptions.Default.WithAddress(newAddress);

        options.ApplyTo(account);

        Assert.AreEqual(name, account.Name);
        Assert.AreEqual(newAddress, account.Address);
        Assert.AreEqual(email, account.Email);
        Assert.AreEqual(phoneNumber, account.PhoneNumber);
    }

    [TestMethod]
    public void ApplyTo_ShouldChangeOnlyName_WhenOnlyNameHasChanges()
    {
        var name = "namee";
        var address = "Maple Street no. 12";
        string? email = "test@gmail.com";
        string? phoneNumber = "0765123123";
        var account = new Account(name, address, email, phoneNumber);
        var newName = "otherName";
        var options = AccountUpdateOptions.Default.WithName(newName);

        options.ApplyTo(account);

        Assert.AreEqual(newName, account.Name);
        Assert.AreEqual(address, account.Address);
        Assert.AreEqual(email, account.Email);
        Assert.AreEqual(phoneNumber, account.PhoneNumber);
    }

    [TestMethod]
    public void ApplyTo_ShouldChangeAllProperties_WhenOriginalAccountContainsNullProperties()
    {
        var name = "namee";
        var address = "Maple Street no. 12";
        string? email = "test@gmail.com";
        string? phoneNumber = null;
        var account = new Account(name, address, email, phoneNumber);
        var newName = "otherName";
        var newAddress = "SomeOtherMaple Street no. 122";
        string? newEmail = null;
        string? newPhoneNumber = "+40765123123";
        var options = AccountUpdateOptions.Default
            .WithName(newName)
            .WithAddress(newAddress)
            .WithEmail(newEmail)
            .WithPhoneNumber(newPhoneNumber);

        options.ApplyTo(account);

        Assert.AreEqual(newName, account.Name);
        Assert.AreEqual(newAddress, account.Address);
        Assert.AreEqual(newEmail, account.Email);
        Assert.AreEqual(newPhoneNumber, account.PhoneNumber);
    }

    [TestMethod]
    public void ApplyTo_ShouldChangeProperties_WhenOptionsContainsNullChanges()
    {
        var name = "namee";
        var address = "Maple Street no. 12";
        string? email = "test@gmail.com";
        string? phoneNumber = "0765123123";
        var account = new Account(name, address, email, phoneNumber);
        var newName = "otherName";
        var newAddress = "SomeOtherMaple Street no. 122";
        string? newEmail = null;
        string? newPhoneNumber = null;
        var options = AccountUpdateOptions.Default
            .WithName(newName)
            .WithAddress(newAddress)
            .WithEmail(newEmail)
            .WithPhoneNumber(newPhoneNumber);

        options.ApplyTo(account);

        Assert.AreEqual(newName, account.Name);
        Assert.AreEqual(newAddress, account.Address);
        Assert.AreEqual(newEmail, account.Email);
        Assert.AreEqual(newPhoneNumber, account.PhoneNumber);
    }

    [TestMethod]
    public void ApplyTo_ShouldDoNothing_WhenOptionsHasNoChanges()
    {
        var name = "namee";
        var address = "Maple Street no. 12";
        string? email = "test@gmail.com";
        string? phoneNumber = "0765123123";
        var account = new Account(name, address, email, phoneNumber);
        var options = AccountUpdateOptions.Default;

        options.ApplyTo(account);

        Assert.AreEqual(name, account.Name);
        Assert.AreEqual(address, account.Address);
        Assert.AreEqual(email, account.Email);
        Assert.AreEqual(phoneNumber, account.PhoneNumber);
    }

    [TestMethod]
    public void ApplyTo_ShouldChangeAllProperties_WhenOptionsHasChangesForAll()
    {
        var name = "namee";
        var address = "Maple Street no. 12";
        string? email = "test@gmail.com";
        string? phoneNumber = "0765123123";
        var account = new Account(name, address, email, phoneNumber);
        var newName = "otherName";
        var newAddress = "SomeOtherMaple Street no. 122";
        string? newEmail = "otherTest@gmail.com";
        string? newPhoneNumber = "+40765123123";
        var options = AccountUpdateOptions.Default
            .WithName(newName)
            .WithAddress(newAddress)
            .WithEmail(newEmail)
            .WithPhoneNumber(newPhoneNumber);

        options.ApplyTo(account);

        Assert.AreEqual(newName, account.Name);
        Assert.AreEqual(newAddress, account.Address);
        Assert.AreEqual(newEmail, account.Email);
        Assert.AreEqual(newPhoneNumber, account.PhoneNumber);
    }
}