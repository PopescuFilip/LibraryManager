namespace DataMapper.QueryHelpers;

public static class Collector<T>
{
    public static readonly Func<IQueryable<T>, List<T>> ToList = q => [.. q];

    public static readonly Func<IQueryable<T>, T?> SingleOrDefault = q => q.SingleOrDefault();
}