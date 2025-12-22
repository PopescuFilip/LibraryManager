using FluentValidation;

namespace ServiceLayer;

public class EmptyValidator<T> : AbstractValidator<T> {}

public static class EmptyValidator
{
    public static IValidator<T> Create<T>() => new EmptyValidator<T>();
}