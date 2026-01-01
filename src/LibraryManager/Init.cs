using DataMapper;
using DomainModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ServiceLayer.CRUD;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using System.Linq.Expressions;

namespace LibraryManager;

public static class Init
{
    public static void Initialize(this Container container)
    {
        using var scope = AsyncScopedLifestyle.BeginScope(container);
        scope.GetRequiredService<LibraryDbContext>().Database.Migrate();

        container.InitDomains();
    }

    public static List<T> GetAllEntities<T>(this IServiceProvider serviceProvider,
        params Expression<Func<T, object?>>[] includeProperties)
        where T : IEntity<int>
        =>
        serviceProvider
        .GetRequiredService<IEntityService<int, T>>()
        .Get(
            select: x => x,
            collector: q => q.ToList(),
            asNoTracking: false,
            includeProperties: includeProperties);
}