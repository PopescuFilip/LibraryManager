namespace DomainModel.Restrictions;

public static class RestrictionsExtensions
{
    public static BookRestrictions ToBookRestrictions(this Restrictions restrictions) =>
        new(restrictions.MaxDomains);
}