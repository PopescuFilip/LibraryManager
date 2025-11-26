namespace DomainModel.Restrictions;

public enum TimeUnit { Day, Month };

public readonly record struct Limit(int ItemCount, int TimeUnitCount, TimeUnit TimeUnit)
{
    public static Limit PerMonth(int ItemCount, int TimeUnitCount) =>
        new(ItemCount, TimeUnitCount, TimeUnit.Month);
    public static Limit PerDay(int ItemCount, int TimeUnitCount) =>
        new(ItemCount, TimeUnitCount, TimeUnit.Day);
}