using DomainModel;
using ServiceLayer.CRUD;

namespace ServiceLayer.BookEditions;

public interface IBookEditionService
{
    Result<BookEdition> Create(string name, int pagesCount, BookType bookType, int bookDefinitionId);
}

public class BookEditionService(
    IEntityService<BookEdition> _entityService,
    IEntityService<BookDefinition> _bookDefinitionService)
    : IBookEditionService
{
    public Result<BookEdition> Create(string name, int pagesCount, BookType bookType, int bookDefinitionId)
    {
        var bookDefinition = _bookDefinitionService.GetById(bookDefinitionId);

        if (bookDefinition is null)
            return Result.Invalid();

        var bookEdition = new BookEdition(name, pagesCount, bookType, bookDefinitionId);
        return _entityService.Insert(bookEdition, EmptyValidator.Create<BookEdition>());
    }
}