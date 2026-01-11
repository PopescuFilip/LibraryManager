using DomainModel;
using FluentValidation.TestHelper;
using ServiceLayer.Accounts;
using System.Diagnostics.CodeAnalysis;

namespace ServiceLayer.UnitTests;

[TestClass]
[ExcludeFromCodeCoverage]
public class AccountValidatorTests
{
    private AccountValidator _validator = default!;

    [TestInitialize]
    public void Init()
    {
        _validator = new AccountValidator();
    }

    [TestMethod]
    public void Validator_ShouldReturnInvalid_WhenEmailIsIncorrect()
    {
        var name = "name";
        var address = "Maple Street no. 12";
        string? email = "test@.com";
        string? phoneNumber = "0765123123";
        var account = new Account(name, address, email, phoneNumber);

        var result = _validator.TestValidate(account);

        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [TestMethod]
    public void Validator_ShouldReturnInvalid_WhenPhoneNumberContainsNonDigitCharacters()
    {
        var name = "name";
        var address = "Maple Street no. 12";
        string? email = null;
        string? phoneNumber = "0765123ab3";
        var account = new Account(name, address, email, phoneNumber);

        var result = _validator.TestValidate(account);

        result.ShouldHaveValidationErrorFor(x => x.PhoneNumber);
    }

    [TestMethod]
    public void Validator_ShouldReturnInvalid_WhenPhoneNumberHasTooManyDigits()
    {
        var name = "name";
        var address = "Maple Street no. 12";
        string? email = "test@gmail.com";
        string? phoneNumber = "0765123123456";
        var account = new Account(name, address, email, phoneNumber);

        var result = _validator.TestValidate(account);

        result.ShouldHaveValidationErrorFor(x => x.PhoneNumber);
    }

    [TestMethod]
    public void Validator_ShouldReturnInvalid_WhenPhoneNumberHasTooFewDigits()
    {
        var name = "name";
        var address = "Maple Street no. 12";
        string? email = null;
        string? phoneNumber = "07651231";
        var account = new Account(name, address, email, phoneNumber);

        var result = _validator.TestValidate(account);

        result.ShouldHaveValidationErrorFor(x => x.PhoneNumber);
    }

    [TestMethod]
    public void Validator_ShouldReturnInvalid_WhenPhoneNumberHasFirstCharPlusAndIsNotInValidState()
    {
        var name = "namee";
        var address = "Maple Street no. 12";
        string? email = "test@gmail.com";
        string? phoneNumber = "+40765123+23";
        var account = new Account(name, address, email, phoneNumber);

        var result = _validator.TestValidate(account);

        result.ShouldHaveValidationErrorFor(x => x.PhoneNumber);
    }

    [TestMethod]
    public void Validator_ShouldReturnInvalid_WhenBothEmailAndPhoneAreNull()
    {
        var name = "name";
        var address = "Maple Street no. 12";
        string? email = null;
        string? phoneNumber = null;
        var account = new Account(name, address, email, phoneNumber);

        var result = _validator.TestValidate(account);

        result.ShouldHaveValidationErrorFor(x => x.Email);
        result.ShouldHaveValidationErrorFor(x => x.PhoneNumber);
    }

    [TestMethod]
    public void Validator_ShouldReturnInvalid_WhenAddressIsEmpty()
    {
        var name = "name";
        var address = "";
        string? email = "test@gmail.com";
        string? phoneNumber = "0765123123";
        var account = new Account(name, address, email, phoneNumber);

        var result = _validator.TestValidate(account);

        result.ShouldHaveValidationErrorFor(x => x.Address);
    }

    [TestMethod]
    public void Validator_ShouldReturnInvalid_WhenNameIsEmpty()
    {
        var name = string.Empty;
        var address = "Maple Street no. 12";
        string? email = "test@gmail.com";
        string? phoneNumber = "0765123123";
        var account = new Account(name, address, email, phoneNumber);

        var result = _validator.TestValidate(account);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [TestMethod]
    public void Validator_ShouldReturnValid_WhenPhoneNumberHasFirstCharPlusAndIsInValidState()
    {
        var name = "namee";
        var address = "Maple Street no. 12";
        string? email = "test@gmail.com";
        string? phoneNumber = "+40765123123";
        var account = new Account(name, address, email, phoneNumber);

        var result = _validator.TestValidate(account);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [TestMethod]
    public void Validator_ShouldReturnValid_WhenOptionsAreInCorrectState()
    {
        var name = "namee";
        var address = "Maple Street no. 12";
        string? email = "test@gmail.com";
        string? phoneNumber = "0765123123";
        var account = new Account(name, address, email, phoneNumber);

        var result = _validator.TestValidate(account);

        result.ShouldNotHaveAnyValidationErrors();
    }
}