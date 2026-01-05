using FluentValidation.Results;
using System.Diagnostics.CodeAnalysis;

namespace ServiceLayer.UnitTests.TestHelpers;

[ExcludeFromCodeCoverage]
public static class Validation
{
    public static readonly ValidationResult ValidResult = new();

    public static readonly ValidationResult InvalidResult =
        new(new List<ValidationFailure>() { new() });
}