using DomainModel;
using FluentValidation;
using ServiceLayer.CRUD;

namespace ServiceLayer.BookEditions;

public interface IBookEditionService
{
    Result<BookEdition> Create(string name, int pagesCount, BookType bookType, int bookDefinitionId);

    Result<BookEdition> AddBooks(BooksUpdateOptions options);

    Result<BookEdition> RemoveBooks(BooksUpdateOptions options);
}

public class BookEditionService(
    IEntityService<BookEdition> _entityService,
    IEntityService<BookDefinition> _bookDefinitionService,
    IValidator<BookEdition> _validator,
    IValidator<BooksUpdateOptions> _optionsValidator,
    IBookEditionQueryService _queryService)
    : IBookEditionService
{
    public Result<BookEdition> Create(string name, int pagesCount, BookType bookType, int bookDefinitionId)
    {
        var bookDefinition = _bookDefinitionService.GetById(bookDefinitionId);

        if (bookDefinition is null)
            return Result.Invalid();

        var bookEdition = new BookEdition(name, pagesCount, bookType, bookDefinitionId);
        return _entityService.Insert(bookEdition, _validator);
    }

    public Result<BookEdition> AddBooks(BooksUpdateOptions options)
    {
        if (!_optionsValidator.Validate(options).IsValid)
            return Result.Invalid();

        var bookEdition = _queryService.GetByIdWithBooks(options.BookEditionId);

        if (bookEdition is null)
            return Result.Invalid();

        bookEdition.AddBooks(options.ToStatusCountDictionary());

        return _entityService.Update(bookEdition, _validator);
    }

    public Result<BookEdition> RemoveBooks(BooksUpdateOptions options)
    {
        if (!_optionsValidator.Validate(options).IsValid)
            return Result.Invalid();

        var bookEdition = _queryService.GetByIdWithBooks(options.BookEditionId);

        if (bookEdition is null)
            return Result.Invalid();

        if (!bookEdition.TryRemoveBooks(options.ToStatusCountDictionary()))
            return Result.Invalid();

        return _entityService.Update(bookEdition, _validator);
    }
}