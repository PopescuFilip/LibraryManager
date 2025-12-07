using Microsoft.Extensions.Configuration;

namespace DataMapper.MigrationHelpers;

public static class AppSettings
{
    private const string AppSettingsFile = "appsettings.json";

    public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
        .AddJsonFile(AppSettingsFile)
        .Build();
}