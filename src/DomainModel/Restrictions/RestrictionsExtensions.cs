namespace DomainModel.Restrictions;

public static class RestrictionsExtensions
{
    public static BookRestrictions ToBookRestrictions(this RawRestrictions restrictions) =>
        new(restrictions.MaxDomains);
}