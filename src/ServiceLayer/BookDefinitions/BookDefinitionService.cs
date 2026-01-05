using DomainModel;
using FluentValidation;
using ServiceLayer.CRUD;

namespace ServiceLayer.BookDefinitions;

public interface IBookDefinitionService
{
    Result<BookDefinition> Create(BookDefinitionCreateOptions options);
}

public class BookDefinitionService(
    IEntityService<BookDefinition> _entityService,
    IEntityService<Author> _authorEntityService,
    IEntityService<Domain> _domainEntityService,
    IValidator<BookDefinitionCreateOptions> _optionsValidator,
    IValidator<BookDefinition> _validator)
    : IBookDefinitionService
{
    public Result<BookDefinition> Create(BookDefinitionCreateOptions options)
    {
        if (!_optionsValidator.Validate(options).IsValid)
            return Result.Invalid();

        var (name, authorIds, domainIds) = options;
        var authors = _authorEntityService.GetAllById(authorIds);
        if (authors.Count != authorIds.Length)
            return Result.Invalid();

        var domains = _domainEntityService.GetAllById(domainIds);
        if (domains.Count != domainIds.Length)
            return Result.Invalid();

        var bookDefinition = new BookDefinition(name, authors, domains);
        return _entityService.Insert(bookDefinition, _validator);
    }
}