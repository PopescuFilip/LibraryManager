using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DataMapper;

public class LibraryDbContextFactory : IDesignTimeDbContextFactory<LibraryDbContext>
{
    private readonly DbContextOptions<LibraryDbContext> _options =
        new DbContextOptionsBuilder<LibraryDbContext>()
        .UseSqlServer(AppSettings.LibraryDbConnectionString)
        .Options;

    public LibraryDbContext CreateDbContext(string[] args) => new(_options);
}