namespace DomainModel.Restrictions;

public static class LimitExtensions
{
    public static T DoubleItem<T>(this T limit) where T : BaseLimit =>
        limit with { ItemCount = limit.ItemCount * 2 };

    public static PeriodLimit HalfTime(this PeriodLimit limit) => limit switch
    {
        (1, _, _) => limit,
        _ => limit with { TimeUnitCount = limit.TimeUnitCount / 2 },
    };

    public static DateTime GetStartTimeToCheck(this TimewiseLimit limit) => limit switch
    {
        PeriodLimit { TimeUnit: TimeUnit.Day } periodLimit => DateTime.Now.AddDays(-periodLimit.TimeUnitCount),
        PeriodLimit { TimeUnit: TimeUnit.Month } periodLimit => DateTime.Now.AddMonths(-periodLimit.TimeUnitCount),
        PerDayLimit => DateTime.Today,
        _ => throw new InvalidOperationException($"{limit} is not supported")
    };
}