using DataMapper;
using DomainModel.Restrictions;

namespace ServiceLayer.CRUD;

public interface IBookRestrictionsProvider
{
    BookRestrictions Get();
}

public class BookRestrictionsProvider(IRestrictionsProvider _restrictionsProvider)
    : IBookRestrictionsProvider
{
    public BookRestrictions Get() => _restrictionsProvider
        .GetRestrictions()!
        .ToBookRestrictions();
}