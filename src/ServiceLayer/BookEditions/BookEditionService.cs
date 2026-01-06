using DomainModel;
using FluentValidation;
using ServiceLayer.CRUD;

namespace ServiceLayer.BookEditions;

public interface IBookEditionService
{
    Result<BookEdition> Create(string name, int pagesCount, BookType bookType, int bookDefinitionId);

    Result<BookEdition> AddBooks(BookAddOptions options);
}

public class BookEditionService(
    IEntityService<BookEdition> _entityService,
    IEntityService<BookDefinition> _bookDefinitionService,
    IValidator<BookEdition> _validator,
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

    public Result<BookEdition> AddBooks(BookAddOptions options)
    {
        var _optionsValidator = EmptyValidator.Create<BookAddOptions>();

        if (!_optionsValidator.Validate(options).IsValid)
            return Result.Invalid();

        var (forReadingCount, forBorrowCount, editionId) = options;

        var bookEdition = _queryService.GetByIdWithBooks(editionId);

        if (bookEdition is null)
            return Result.Invalid();

        bookEdition.AddBooks(BookStatus.ForReadingRoom, forReadingCount);
        bookEdition.AddBooks(BookStatus.Available, forBorrowCount);

        return _entityService.Update(bookEdition, _validator);
    }
}