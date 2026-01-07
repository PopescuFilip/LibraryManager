namespace ServiceLayer.Authors;

public record AuthorOptions(
    string Name,
    string Address,
    string? Email,
    string? PhoneNumber);