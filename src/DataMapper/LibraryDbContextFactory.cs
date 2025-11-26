using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataMapper;

public class LibraryDbContextFactory(IConfiguration configuration) : IDbContextFactory<LibraryDbContext>
{
    private readonly DbContextOptions<LibraryDbContext> _options =
        DbContextOptionsCreator.Create(configuration);

    public LibraryDbContext CreateDbContext() => new(_options);
}