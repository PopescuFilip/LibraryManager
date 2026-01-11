using DomainModel;
using FluentValidation;

namespace ServiceLayer.Borrowing;

public class ExtensionValidator : AbstractValidator<Extension>
{
    public ExtensionValidator()
    {
        RuleFor(x => x.CreatedDateTime).Must(x => DateTime.Now >= x);
        RuleFor(x => x.DayCount).GreaterThan(0);
    }
}