using DataMapper;
using DomainModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ServiceLayer.CRUD;
using SimpleInjector;
using System.Linq.Expressions;

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

    public static List<T> GetAllEntities<T>(this Container container,
        params Expression<Func<T, object?>>[] includeProperties)
        where T : IEntity<int>
        =>
        container
        .GetRequiredService<IEntityService<IRepository<int, T>, int, T>>()
        .Get(
            select: x => x,
            collector: q => q.ToList(),
            includeProperties: includeProperties);
}