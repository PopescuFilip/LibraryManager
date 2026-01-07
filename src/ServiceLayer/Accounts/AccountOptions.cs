using DomainModel;

namespace ServiceLayer.Accounts;

public record AccountUpdateOptions(
    Optional<string> NewName,
    Optional<string> NewAddress,
    Optional<string?> NewEmail,
    Optional<string?> NewPhoneNumber);

public record AccountOptions(
    string Name,
    string Address,
    string? Email,
    string? PhoneNumber);

public static class AccountOptionsExtensions
{
    public static Account ToAccount(this AccountOptions options) =>
        new(options.Name, options.Address, options.Email, options.PhoneNumber);

    public static void ApplyTo(this AccountUpdateOptions options, Account account)
    {
        options.NewName.Apply(x => account.Name = x);
        options.NewAddress.Apply(x => account.Address = x);
        options.NewEmail.Apply(x => account.Email = x);
        options.NewPhoneNumber.Apply(x => account.PhoneNumber = x);
    }
}