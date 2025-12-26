using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DataMapper;

public static class DbContextOptionsCreator
{
    private const string ConnectionStringName = "LibraryDb";

    public static DbContextOptions<LibraryDbContext> Create(IConfiguration configuration) =>
        new DbContextOptionsBuilder<LibraryDbContext>()
        .UseSqlServer(configuration.GetConnectionString(ConnectionStringName))
        .EnableSensitiveDataLogging()
        .LogTo(Console.WriteLine, minimumLevel: LogLevel.Information)
        .Options;
}
