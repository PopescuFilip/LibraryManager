namespace ServiceLayer.Accounts;

public record AccountOptions(
    string Name,
    string Address,
    string? Email,
    string? PhoneNumber);