using DataMapper;
using DataMapper.QueryHelpers;
using DomainModel;

namespace ServiceLayer.CRUD;

public interface IBookEditionQueryService
{
    BookEdition? GetByIdWithBooks(int id);
}

public class BookEditionQueryService(IRepository<BookEdition> _repository)
    : IBookEditionQueryService
{
    public BookEdition? GetByIdWithBooks(int id) =>
        _repository.Get(
            Select<BookEdition>.Default,
            Collector<BookEdition>.SingleOrDefault,
            asNoTracking: false,
            Order<BookEdition>.ById,
            filter: x => x.Id == id,
            includeProperties: x => x.BookRecords);
}