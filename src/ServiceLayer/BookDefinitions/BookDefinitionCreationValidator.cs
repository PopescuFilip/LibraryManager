using FluentValidation;

namespace ServiceLayer.BookDefinitions;

public class BookDefinitionCreationValidator : AbstractValidator<BookDefinitionCreateOptions>
{
    public BookDefinitionCreationValidator(IValidator<IdCollection> idsValidator)
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.AuthorIds).SetValidator(idsValidator);
        RuleFor(x => x.DomainIds).SetValidator(idsValidator);
    }
}