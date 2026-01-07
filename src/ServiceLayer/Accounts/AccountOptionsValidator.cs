using FluentValidation;
using System.Net.Mail;

namespace ServiceLayer.Accounts;

public class AccountOptionsValidator : AbstractValidator<AccountOptions>
{
    private const char Plus = '+';

    public AccountOptionsValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Address).NotEmpty();

        RuleFor(x => x.PhoneNumber).NotEmpty()
            .When(x => x.Email is null);
        RuleFor(x => x.PhoneNumber).Length(10, 12)
            .When(x => x.PhoneNumber is not null);
        RuleFor(x => x.PhoneNumber).Must(IsValidPhoneNumber!)
            .When(x => x.PhoneNumber is not null);

        RuleFor(x => x.Email).NotEmpty()
            .When(x => x.PhoneNumber is null);
        RuleFor(x => x.Email).Must(IsValidEmail!)
            .When(x => x.Email is not null);
    }

    public static bool IsValidPhoneNumber(string phoneNumber) =>
        phoneNumber.StartsWith(Plus)
        ? phoneNumber.Skip(1).All(char.IsDigit)
        : phoneNumber.All(char.IsDigit);

    public static bool IsValidEmail(string email) =>
        MailAddress.TryCreate(email, out var _);
}