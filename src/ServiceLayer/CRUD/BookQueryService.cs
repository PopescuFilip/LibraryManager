using DataMapper;
using DataMapper.QueryHelpers;
using DomainModel;
using DomainModel.DTOs;
using System.Collections.Immutable;

namespace ServiceLayer.CRUD;

public interface IBookQueryService
{
    IReadOnlyCollection<BookDetails> GetBookDetails(IdCollection ids);
}

public class BookQueryService(IRepository<Book> _repository) : IBookQueryService
{
    public IReadOnlyCollection<BookDetails> GetBookDetails(IdCollection ids)
    {
        return _repository.Get(
            x => new BookDetails(
                x.Id,
                x.BookEditionId,
                x.BookEdition.BookDefinition.Domains.Select(x => x.Id).ToImmutableArray()),
            Collector<BookDetails>.ToList,
            asNoTracking: true,
            Order<Book>.ById,
            filter: x => ids.Contains(x.Id),
            includeProperties: [x => x.BookEdition.BookDefinition.Domains]);
    }
}