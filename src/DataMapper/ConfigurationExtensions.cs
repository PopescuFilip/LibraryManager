using DomainModel.Restrictions;
using Microsoft.Extensions.Configuration;

namespace DataMapper;

public static class ConfigurationExtensions
{
    private const string RestrictionsSection = "Restrictions";

    public static Restrictions? GetRestrictions(this IConfiguration configuration) =>
        configuration.GetRequiredSection(RestrictionsSection).Get<Restrictions>();
}