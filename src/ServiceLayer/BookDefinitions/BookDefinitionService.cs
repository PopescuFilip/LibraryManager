using DomainModel;
using ServiceLayer.CRUD;

namespace ServiceLayer.BookDefinitions;

public interface IBookDefinitionService
{
    Result<BookDefinition> Create(string name, List<int> authorIds, List<int> domainIds);
}

public class BookDefinitionService(
    IEntityService<int, BookDefinition> _entityService,
    IEntityService<int, Author> _authorEntityService,
    IEntityService<int, Domain> _domainEntityService)
    : IBookDefinitionService
{
    public Result<BookDefinition> Create(string name, List<int> authorIds, List<int> domainIds)
    {
        var authors = _authorEntityService.GetAllById(authorIds);
        if (authors.Count != authorIds.Count)
            return Result.Invalid();

        var domains = _domainEntityService.GetAllById(domainIds);
        if (domains.Count != domainIds.Count)
            return Result.Invalid();

        var entitiesToAttach = authors.Cast<object>().Concat(domains).ToArray();

        var bookDefinition = new BookDefinition(name, authors, domains);
        return _entityService.Insert(bookDefinition,
            EmptyValidator.Create<BookDefinition>(),
            entitiesToAttach);
    }
}