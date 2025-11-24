using Microsoft.Extensions.Configuration;

namespace DataMapper;

public static class AppSettings
{
    private const string AppSettingsFile = "appsettings.json";
    private const string ConnectionStringName = "LibraryDb";
    private const string RestrictionsSection = "Restrictions";

    public static string LibraryDbConnectionString { get; private set; }
    public static RawRestrictions Restrictions { get; private set; }

    static AppSettings()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile(AppSettingsFile)
            .Build();

        LibraryDbConnectionString = configuration.GetConnectionString(ConnectionStringName)!;
        Restrictions = configuration.GetRequiredSection(RestrictionsSection).Get<RawRestrictions>()!;
    }
}