namespace ServiceLayer;

public abstract record Optional<T>;

public sealed record Some<T>(T Value) : Optional<T>();

public sealed record None<T>() : Optional<T>();

public static class Optional
{
    public static Optional<T> Some<T>(T value) => new Some<T>(value);

    public static Optional<T> None<T>() => new None<T>();

    public static void ApplySet<T>(this Optional<T> optional, T value) =>
        optional.Apply(x => value = x);

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
}