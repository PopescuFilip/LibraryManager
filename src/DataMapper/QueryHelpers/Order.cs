using DomainModel;

namespace DataMapper.QueryHelpers;

public static class Order
{
    public static Func<IQueryable<T>, IOrderedQueryable<T>> ById<T>()
        where T : IEntity<int> =>
        q => q.OrderBy(x => x.Id);
}