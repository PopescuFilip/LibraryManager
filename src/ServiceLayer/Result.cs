namespace ServiceLayer;

public record Result<T>(T? Value, bool IsValid)
{
    public T Get() => Value is null ? throw new NullReferenceException() : Value;

    public static Result<T> Valid(T Value) => new(Value, true);

    public static Result<T> Invalid() => new(default, false);
}