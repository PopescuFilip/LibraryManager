using DomainModel;

namespace ServiceLayer.Accounts;

public record AccountUpdateOptions(
    Optional<string> Name,
    Optional<string> Address,
    Optional<string?> Email,
    Optional<string?> PhoneNumber);

public static class AccountUpdateOptionsExtensions
{
    public static void ApplyTo(this AccountUpdateOptions options, Account account)
    {
        options.Name.Apply(x => account.Name = x);
        options.Address.Apply(x => account.Address = x);
        options.Email.Apply(x => account.Email = x);
        options.PhoneNumber.Apply(x => account.PhoneNumber = x);
    }
}