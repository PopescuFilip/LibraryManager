using ServiceLayer.Accounts;
using System.Diagnostics.CodeAnalysis;

namespace ServiceLayer.UnitTests;

[TestClass]
[ExcludeFromCodeCoverage]
public class AccountUpdateOptionsTests
{
    [TestMethod]
    public void Default_ShouldContaintOnlyNone()
    {
        var options = AccountUpdateOptions.Default;

        Assert.IsTrue(options.Name is None<string>);
        Assert.IsTrue(options.Address is None<string>);
        Assert.IsTrue(options.PhoneNumber is None<string?>);
        Assert.IsTrue(options.Email is None<string?>);
    }

    [TestMethod]
    public void WithEmail_ShouldWork_WhenCalledWithNull()
    {
        var options = AccountUpdateOptions.Default;

        options = options.WithEmail(null);

        Assert.IsTrue(options.Email is Some<string?>);
        Assert.IsNull(options.Email.Get());
        Assert.IsTrue(options.Name is None<string>);
        Assert.IsTrue(options.Address is None<string>);
        Assert.IsTrue(options.PhoneNumber is None<string?>);
    }

    [TestMethod]
    public void WithPhoneNumber_ShouldWork_WhenCalledWithNull()
    {
        var options = AccountUpdateOptions.Default;

        options = options.WithPhoneNumber(null);

        Assert.IsTrue(options.PhoneNumber is Some<string?>);
        Assert.IsNull(options.PhoneNumber.Get());
        Assert.IsTrue(options.Name is None<string>);
        Assert.IsTrue(options.Address is None<string>);
        Assert.IsTrue(options.Email is None<string?>);
    }

    [TestMethod]
    public void WithEmail_ShouldChangeOnlyEmail()
    {
        var options = AccountUpdateOptions.Default;
        string? email = "test@gmail.com";

        options = options.WithEmail(email);

        Assert.IsTrue(options.Email is Some<string?>);
        Assert.AreEqual(email, options.Email.Get());
        Assert.IsTrue(options.Name is None<string>);
        Assert.IsTrue(options.Address is None<string>);
        Assert.IsTrue(options.PhoneNumber is None<string?>);
    }

    [TestMethod]
    public void WithPhoneNumber_ShouldChangeOnlyPhoneNumber()
    {
        var options = AccountUpdateOptions.Default;
        string? phoneNumber = "0765123123";

        options = options.WithPhoneNumber(phoneNumber);

        Assert.IsTrue(options.PhoneNumber is Some<string?>);
        Assert.AreEqual(phoneNumber, options.PhoneNumber.Get());
        Assert.IsTrue(options.Name is None<string>);
        Assert.IsTrue(options.Address is None<string>);
        Assert.IsTrue(options.Email is None<string?>);
    }

    [TestMethod]
    public void WithAddress_ShouldChangeOnlyAddress()
    {
        var options = AccountUpdateOptions.Default;
        var address = "Maple Street no. 12";

        options = options.WithAddress(address);

        Assert.IsTrue(options.Address is Some<string>);
        Assert.AreEqual(address, options.Address.Get());
        Assert.IsTrue(options.Name is None<string>);
        Assert.IsTrue(options.PhoneNumber is None<string?>);
        Assert.IsTrue(options.Email is None<string?>);
    }

    [TestMethod]
    public void WithName_ShouldChangeOnlyName()
    {
        var options = AccountUpdateOptions.Default;
        var name = "namee";

        options = options.WithName(name);

        Assert.IsTrue(options.Name is Some<string>);
        Assert.AreEqual(name, options.Name.Get());
        Assert.IsTrue(options.Address is None<string>);
        Assert.IsTrue(options.PhoneNumber is None<string?>);
        Assert.IsTrue(options.Email is None<string?>);
    }

    [TestMethod]
    public void WithMethods_ShouldWorkWhenCalledTogether()
    {
        var options = AccountUpdateOptions.Default;
        var name = "namee";
        var address = "Maple Street no. 12";
        string? email = "test@gmail.com";
        string? phoneNumber = "0765123123";

        options = options
            .WithName(name)
            .WithAddress(address)
            .WithEmail(email)
            .WithPhoneNumber(phoneNumber);

        Assert.IsTrue(options.Name is Some<string>);
        Assert.AreEqual(name, options.Name.Get());
        Assert.IsTrue(options.Address is Some<string>);
        Assert.AreEqual(address, options.Address.Get());
        Assert.IsTrue(options.PhoneNumber is Some<string?>);
        Assert.AreEqual(phoneNumber, options.PhoneNumber.Get());
        Assert.IsTrue(options.Email is Some<string?>);
        Assert.AreEqual(email, options.Email.Get());
    }
}