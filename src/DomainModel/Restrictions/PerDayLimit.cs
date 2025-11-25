namespace DomainModel.Restrictions;

public record class PerDayLimit(int Limit, int DayCount);
public record class PerMonthLimit(int Limit, int MonthCount);