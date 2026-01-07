using DomainModel;

namespace ServiceLayer.Accounts;

public record AccountUpdateOptions(
    Optional<string> Name,
    Optional<string> Address,
    Optional<string?> Email,
    Optional<string?> PhoneNumber)
{
    public static readonly AccountUpdateOptions Empty = new(
        Optional.None<string>(),
        Optional.None<string>(),
        Optional.None<string?>(),
        Optional.None<string?>());

    public AccountUpdateOptions WithName(string name) =>
        this with { Name = Optional.Some(name) };

    public AccountUpdateOptions WithAddress(string address) =>
        this with { Address = Optional.Some(address) };

    public AccountUpdateOptions WithEmail(string? email) =>
        this with { Email = Optional.Some(email) };

    public AccountUpdateOptions WithPhoneNumber(string? phoneNumber) =>
        this with { PhoneNumber = Optional.Some(phoneNumber) };
}

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