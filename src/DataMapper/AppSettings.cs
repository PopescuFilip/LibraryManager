using Microsoft.Extensions.Configuration;

namespace DataMapper;

public static class AppSettings
{
    private const string AppSettingsFile = "appsettings.json";
    private const string ConnectionStringName = "LibraryDb";

    public static string LibraryDbConnectionString { get; private set; }

    static AppSettings()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile(AppSettingsFile)
            .Build();

        LibraryDbConnectionString = configuration.GetConnectionString(ConnectionStringName)!;
    }
}