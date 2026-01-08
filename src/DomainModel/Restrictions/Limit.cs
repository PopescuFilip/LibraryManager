namespace DomainModel.Restrictions;

public enum TimeUnit { Day, Month };

public abstract record BaseLimit(int ItemCount);

public abstract record TimewiseLimit(int ItemCount) : BaseLimit(ItemCount);

public record PerRequestLimit(int ItemCount) : BaseLimit(ItemCount);

public sealed record PerDayLimit(int ItemCount) : TimewiseLimit(ItemCount);

public sealed record PeriodLimit(int ItemCount, int TimeUnitCount, TimeUnit TimeUnit) : TimewiseLimit(ItemCount);

public static class Limit
{
    public static PerDayLimit PerDay(int ItemCount) => new(ItemCount);

    public static PerRequestLimit PerRequest(int ItemCount) => new(ItemCount);

    public static PeriodLimit PerPeriodInDays(int ItemCount, int DayCount) =>
        new(ItemCount, DayCount, TimeUnit.Day);

    public static PeriodLimit PerPeriodInMonths(int ItemCount, int DayCount) =>
        new(ItemCount, DayCount, TimeUnit.Month);
}