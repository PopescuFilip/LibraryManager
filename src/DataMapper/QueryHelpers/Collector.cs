namespace DataMapper.QueryHelpers;

public static class Collector<T>
{
    public static readonly Func<IQueryable<T>, List<T>> ToList = q => [.. q];

    public static readonly Func<IQueryable<T>, T?> SingleOrDefault = q => q.SingleOrDefault();

    public static readonly Func<IQueryable<T>, T?> FirstOrDefault = q => q.FirstOrDefault();

    public static readonly Func<IQueryable<T>, bool> Any = q => q.Any();

    public static readonly Func<IQueryable<T>, int> Count = q => q.Count();
}