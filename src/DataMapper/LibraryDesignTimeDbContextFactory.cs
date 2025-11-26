using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DataMapper;

public class LibraryDesignTimeDbContextFactory : IDesignTimeDbContextFactory<LibraryDbContext>
{
    private readonly DbContextOptions<LibraryDbContext> _options =
        DbContextOptionsCreator.Create(AppSettings.Configuration);

    public LibraryDbContext CreateDbContext(string[] args) => new(_options);
}