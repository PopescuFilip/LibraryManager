using FluentValidation;

namespace ServiceLayer;

public class IdCollectionValidator : AbstractValidator<IdCollection>
{
    public IdCollectionValidator()
    {
        RuleFor(x => x.Count).GreaterThan(0);
        RuleFor(x => x).Must(ids => ids.Count == ids.Distinct().Count());
    }
}