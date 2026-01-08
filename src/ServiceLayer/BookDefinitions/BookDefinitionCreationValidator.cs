using FluentValidation;

namespace ServiceLayer.BookDefinitions;

public class BookDefinitionCreationValidator : AbstractValidator<BookDefinitionCreateOptions>
{
    public BookDefinitionCreationValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.AuthorIds).SetValidator(new IdCollectionValidator());
        RuleFor(x => x.DomainIds).SetValidator(new IdCollectionValidator());
    }
}