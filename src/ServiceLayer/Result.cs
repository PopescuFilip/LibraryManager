namespace ServiceLayer;

public sealed record InvalidResult;

public sealed record Result<T>(T? Value, bool IsValid)
{
    /// <summary>
    /// Checks if <see cref="Value"/> is not null and returns it. Throws otherwise.
    /// </summary>
    /// <returns><see cref="Value"/> non-nullable</returns>
    /// <exception cref="NullReferenceException"></exception>
    public T Get() => Value is null ? throw new NullReferenceException() : Value;

    public static implicit operator Result<T>(InvalidResult _) => new(default, false);
}

public static class Result
{
    public static InvalidResult Invalid() => new();

    public static Result<T> Valid<T>(T Value) => new(Value, true);

    public static Result<T> Invalid<T>() => new(default, false);
}