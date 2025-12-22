// See https://aka.ms/new-console-template for more information
using DataMapper;
using DataMapper.MigrationHelpers;
using DomainModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServiceLayer;
using ServiceLayer.AdvancedQuery;
using SimpleInjector;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        var container = new Container();
        _ = GetEntryPoint(container);
        RegisterAndVerifyAll(container);

        var provider = container.GetRequiredService<IClientRestrictionsProvider>();

        var domain = container.GetRequiredService<IQueryService<int, Domain>>();

        var first = provider.GetClientRestrictions();
        var second = provider.GetPrivilegedClientRestrictions();
        Console.WriteLine(first);
        Console.WriteLine();
        Console.WriteLine(second);
    }

    private static void RegisterAndVerifyAll(Container container)
    {
        AddDataMapperDependencies(container);
        AddServiceLayerDependencies(container);
        container.Verify();
    }

    private static IHost GetEntryPoint(Container container) =>
        Host
        .CreateDefaultBuilder()
        .ConfigureServices((context, services) =>
        {
            services
            .AddSimpleInjector(container)
            .AddDbContextFactory<LibraryDbContext, LibraryDbContextFactory>();
        })
        .ConfigureAppConfiguration(builder =>
        {
            builder.AddConfiguration(AppSettings.Configuration);
        })
        .Build()
        .UseSimpleInjector(container);

    private static void AddDataMapperDependencies(Container container)
    {
        container.Register(typeof(IRepository<,>), typeof(Repository<,>));
        container.Register<IRestrictionsProvider, RestrictionsProvider>();
    }

    private static void AddServiceLayerDependencies(Container container)
    {
        container.Register(typeof(IQueryService<,>), typeof(QueryService<,>));
        container.Register<IClientRestrictionsProvider, ClientRestrictionsProvider>();
    }
}