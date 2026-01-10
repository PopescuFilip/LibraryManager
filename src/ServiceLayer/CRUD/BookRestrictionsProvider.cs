using DataMapper;
using DomainModel.Restrictions;

namespace ServiceLayer.CRUD;

public interface IBookRestrictionsProvider
{
    Result<BookRestrictions> Get();
}

public class BookRestrictionsProvider(IRestrictionsProvider _restrictionsProvider)
    : IBookRestrictionsProvider
{
    public Result<BookRestrictions> Get()
    {
        var restrictions = _restrictionsProvider.GetRestrictions();
        if (restrictions is null)
            return Result.Invalid();

        return Result.Valid(restrictions.ToBookRestrictions());
    }
}