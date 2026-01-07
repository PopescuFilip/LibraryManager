using FluentValidation;

namespace ServiceLayer.Accounts;

public class AccountOptionsValidator : AbstractValidator<AccountOptions>
{
    public AccountOptionsValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}