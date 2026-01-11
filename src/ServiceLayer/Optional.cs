namespace ServiceLayer;

public abstract record Optional<T>(bool HasValue);

public sealed record Some<T>(T Value) : Optional<T>(true);

public sealed record None<T>() : Optional<T>(false);

public static class Optional
{
    public static Optional<T> Some<T>(T value) => new Some<T>(value);

    public static Optional<T> None<T>() => new None<T>();

    public static Optional<T> FromNullable<T>(T? value) =>
        value is null
        ? new None<T>()
        : new Some<T>(value);

    public static void Apply<T>(this Optional<T> optional, Action<T> action)
    {
        switch (optional)
        {
            case Some<T> s:
                action(s.Value);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Checks if the <see cref="Optional{T}"/> is a <see cref="ServiceLayer.Some{T}"/> and returns its containing value. Throws otherwise.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="optional"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static T Get<T>(this Optional<T> optional) =>
        optional is Some<T> some
        ? some.Value
        : throw new InvalidOperationException();

    public static Optional<R> MapToOptional<R, T>(this Optional<T> optional, Func<T, R> transform) =>
        optional.Map(x => Some(transform(x)), new None<R>());

    public static R Map<R, T>(this Optional<T> optional, Func<T, R> transform, R defaultValue) => optional switch
    {
        Some<T> some => transform(some.Value),
        None<T> => defaultValue,
        _ => throw new InvalidOperationException()
    };
}