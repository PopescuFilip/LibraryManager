using DataMapper;
using DataMapper.QueryHelpers;
using DomainModel;
using System.Linq.Expressions;

namespace ServiceLayer.CRUD;

public static class RepositoryExtensions
{
    public static bool Exists<T>(this IRepository<T> repository,
        Expression<Func<T, bool>>? filter = null)
        where T : IEntity
        =>
        repository.Get(
            Select<T>.Id,
            Collector<int>.Any,
            asNoTracking: true,
            Order<T>.ById,
            filter: filter);
}