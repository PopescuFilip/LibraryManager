using DomainModel;

namespace DataMapper.QueryHelpers;

public static class Order<T> where T : IEntity
{
    public static readonly Func<IQueryable<T>, IOrderedQueryable<T>> ById =
        q => q.OrderBy(x => x.Id);
}