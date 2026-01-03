using DomainModel;
using System.Linq.Expressions;

namespace DataMapper.QueryHelpers;

public static class Select<T> where T : IEntity
{
    public static readonly Expression<Func<T, T>> Default = x => x;

    public static readonly Expression<Func<T, int>> Id = x => x.Id;
}