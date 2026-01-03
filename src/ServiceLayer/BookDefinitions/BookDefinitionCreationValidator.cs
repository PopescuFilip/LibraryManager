using FluentValidation;

namespace ServiceLayer.BookDefinitions;

public class BookDefinitionCreationValidator : AbstractValidator<BookDefinitionCreateOptions>
{
    public BookDefinitionCreationValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.AuthorIds.Length).GreaterThan(0);
        RuleFor(x => x.DomainIds.Length).GreaterThan(0);
    }
}