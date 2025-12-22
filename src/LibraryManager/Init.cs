using DataMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;

namespace LibraryManager;

public static class Init
{
    public static void Initialize(this Container container)
    {
        using var dbContext = container.GetRequiredService<IDbContextFactory<LibraryDbContext>>()
            .CreateDbContext();

        dbContext.Database.Migrate();

        container.InitDomains();
    }
}