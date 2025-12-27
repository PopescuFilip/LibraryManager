using DomainModel;
using FluentValidation;

namespace ServiceLayer.Authors;

public class AuthorValidator : AbstractValidator<Author>
{
    public AuthorValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}