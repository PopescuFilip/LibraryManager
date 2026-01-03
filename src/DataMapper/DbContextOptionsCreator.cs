using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DataMapper;

public static class DbContextOptionsCreator
{
    private const string ConnectionStringName = "LibraryDb";

    public static DbContextOptions<LibraryDbContext> Create(IConfiguration configuration)
    {
        var builder = new DbContextOptionsBuilder<LibraryDbContext>();
        builder.Configure(configuration);

        return builder.Options;
    }

    public static void Configure<T>(this T options, IConfiguration configuration)
        where T : DbContextOptionsBuilder
    {
        options
            .UseSqlServer(configuration.GetConnectionString(ConnectionStringName))
            .EnableSensitiveDataLogging()
            .LogTo(Console.WriteLine, minimumLevel: LogLevel.Information);
    }
}