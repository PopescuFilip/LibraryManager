using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DataMapper;

public static class DbContextOptionsCreator
{
    private const string ConnectionStringName = "LibraryDb";

    public static DbContextOptions<LibraryDbContext> Create(IConfiguration configuration) =>
        new DbContextOptionsBuilder<LibraryDbContext>()
        .Configure(configuration)
        .Options;

    public static T Configure<T>(
        this T options,
        IConfiguration configuration)
        where T : DbContextOptionsBuilder =>
        (options
            .UseSqlServer(configuration.GetConnectionString(ConnectionStringName))
            .EnableSensitiveDataLogging()
            .LogTo(Console.WriteLine, minimumLevel: LogLevel.Information) as T)!;
}