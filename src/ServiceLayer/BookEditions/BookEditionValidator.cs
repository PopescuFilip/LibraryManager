using DomainModel;
using FluentValidation;

namespace ServiceLayer.BookEditions;

public class BookEditionValidator : AbstractValidator<BookEdition>
{
    public BookEditionValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.PagesCount).GreaterThan(0);
    }
}