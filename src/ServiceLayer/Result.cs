namespace ServiceLayer;

public record Result<T>(T? Value, bool IsValid)
{
    /// <summary>
    /// Checks if <see cref="Value"/> is not null and returns it. Throws otherwise.
    /// </summary>
    /// <returns><see cref="Value"/> non-nullable</returns>
    /// <exception cref="NullReferenceException"></exception>
    public T Get() => Value is null ? throw new NullReferenceException() : Value;

    public static Result<T> Valid(T Value) => new(Value, true);

    public static Result<T> Invalid() => new(default, false);
}