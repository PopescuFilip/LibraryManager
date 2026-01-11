using DataMapper;
using DataMapper.QueryHelpers;
using DomainModel;
using DomainModel.DTOs;
using System.Collections.Immutable;

namespace ServiceLayer.CRUD;

public interface IBookQueryService
{
    IReadOnlyCollection<BookDetails> GetBookDetails(IdCollection ids);

    IReadOnlyCollection<BookDetails2> GetBookDetails2(IdCollection ids);
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

    public IReadOnlyCollection<BookDetails2> GetBookDetails2(IdCollection ids)
    {
        return _repository.Get(
            x => new BookDetails2(
                x.Id,
                x,
                x.BookEditionId,
                x.BookEdition.BookRecords.Count(x => x.Status == BookStatus.Available),
                x.BookEdition.BookRecords.Count(x =>
                    x.Status == BookStatus.Available ||
                    x.Status == BookStatus.Borrowed),
                x.BookEdition.BookDefinition.Domains.Select(x => x.Id).ToImmutableArray()),
            Collector<BookDetails2>.ToList,
            asNoTracking: false,
            Order<Book>.ById,
            filter: x => ids.Contains(x.Id),
            includeProperties:
            [
                x => x.BookEdition.BookDefinition.Domains,
                x => x.BookEdition.BookRecords
            ]);
    }
}