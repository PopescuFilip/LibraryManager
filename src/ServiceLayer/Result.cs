namespace ServiceLayer;

public record Result(bool IsValid)
{
    public static Result Valid() => new(true);

    public static Result InValid() => new(false);
}

public record Result<T>(bool IsValid, T? Value)
{
    public static Result<T> Valid(T Value) => new(true, Value);

    public static Result<T> InValid() => new(false, default);
}