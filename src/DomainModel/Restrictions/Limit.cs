namespace DomainModel.Restrictions;

public enum TimeUnit { Day, Month };

public abstract record Limit(int ItemCount)
{
    public static PerDayLimit PerDay(int ItemCount) => new(ItemCount);

    public static PerRequestLimit PerRequest(int ItemCount) => new(ItemCount);

    public static PeriodLimit PerPeriodInDays(int ItemCount, int DayCount) =>
        new(ItemCount, DayCount, TimeUnit.Day);

    public static PeriodLimit PerPeriodInMonths(int ItemCount, int DayCount) =>
        new(ItemCount, DayCount, TimeUnit.Month);
}

public record PerRequestLimit(int ItemCount) : Limit(ItemCount);

public abstract record TimewiseLimit(int ItemCount) : Limit(ItemCount)
{
    public DateTime GetStartTimeToCheck() => this switch
    {
        PeriodLimit { TimeUnit: TimeUnit.Day } periodLimit => DateTime.Now.AddDays(-periodLimit.TimeUnitCount),
        PeriodLimit { TimeUnit: TimeUnit.Month } periodLimit => DateTime.Now.AddMonths(-periodLimit.TimeUnitCount),
        PerDayLimit => DateTime.Today,
        _ => throw new InvalidOperationException($"{this} is not supported")
    };
}

public sealed record PerDayLimit(int ItemCount) : TimewiseLimit(ItemCount);

public sealed record PeriodLimit(int ItemCount, int TimeUnitCount, TimeUnit TimeUnit) : TimewiseLimit(ItemCount)
{
    public PeriodLimit HalfTime() => this switch
    {
        (1, _, _) => this,
        _ => this with { TimeUnitCount = TimeUnitCount / 2 },
    };
}

public static class LimitExtensions
{
    public static T DoubleItem<T>(this T limit) where T : Limit =>
        limit with { ItemCount = limit.ItemCount * 2 };
}