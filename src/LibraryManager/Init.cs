using DataMapper;
using DomainModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ServiceLayer.Authors;
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

        if (scope.GetAllEntities<Author>().Count == 0)
        {
            var authorCreator = scope.GetRequiredService<IAuthorService>();
            authorCreator.Create("Name");
            authorCreator.Create("Other name");
        }
    }

    public static List<T> GetAllEntities<T>(this IServiceProvider serviceProvider,
        params Expression<Func<T, object?>>[] includeProperties)
        where T : IEntity<int>
        =>
        serviceProvider
        .GetRequiredService<IRepository<int, T>>()
        .Get(
            select: x => x,
            collector: q => q.ToList(),
            asNoTracking: false,
            orderBy: q => q.OrderBy(x => x.Id),
            includeProperties: includeProperties);
}