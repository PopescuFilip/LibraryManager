using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataMapper;

public static class DbContextOptionsCreator
{
    private const string ConnectionStringName = "LibraryDb";

    public static DbContextOptions<LibraryDbContext> Create(IConfiguration configuration) =>
        new DbContextOptionsBuilder<LibraryDbContext>()
        .UseSqlServer(configuration.GetConnectionString(ConnectionStringName))
        .Options;
}
