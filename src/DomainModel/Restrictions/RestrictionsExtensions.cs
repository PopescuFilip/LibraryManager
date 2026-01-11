namespace DomainModel.Restrictions;

public static class RestrictionsExtensions
{
    public static BookRestrictions ToBookRestrictions(this RawRestrictions restrictions) =>
        new(restrictions.MaxDomains);

    public static EmployeeRestrictions ToEmployeeRestrictions(this RawRestrictions restrictions)
        => new(Limit.PerDay(restrictions.MaxBorrowedBooksGivenPerDay));
}