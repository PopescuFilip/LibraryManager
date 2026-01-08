using DomainModel.Restrictions;
using Microsoft.Extensions.Configuration;

namespace DataMapper;

public interface IRestrictionsProvider
{
    RawRestrictions? GetRestrictions();
}

public class RestrictionsProvider(IConfiguration configuration) : IRestrictionsProvider
{
    private const string RestrictionsSection = "Restrictions";

    public RawRestrictions? GetRestrictions() =>
        configuration.GetRequiredSection(RestrictionsSection).Get<RawRestrictions>();
}