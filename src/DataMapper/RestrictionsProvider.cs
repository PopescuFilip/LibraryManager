using DomainModel.Restrictions;
using Microsoft.Extensions.Configuration;

namespace DataMapper;

public interface IRestrictionsProvider
{
    Restrictions? GetRestrictions();
}

public class RestrictionsProvider(IConfiguration configuration) : IRestrictionsProvider
{
    private const string RestrictionsSection = "Restrictions";

    public Restrictions? GetRestrictions() =>
        configuration.GetRequiredSection(RestrictionsSection).Get<Restrictions>();
}