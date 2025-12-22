// See https://aka.ms/new-console-template for more information
using DataMapper;
using DataMapper.MigrationHelpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServiceLayer.CRUD;
using ServiceLayer.Domains;
using SimpleInjector;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        var container = new Container();
        _ = GetEntryPoint(container);
        RegisterAndVerifyAll(container);

        var domainService = container.GetRequiredService<IDomainService>();

        var success = domainService.Add("Chimie", "Stiinta");

        Console.WriteLine(success ? "Success" : "Not success");
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
        container.Register<IDomainRepository, DomainRepository>();
    }

    private static void AddServiceLayerDependencies(Container container)
    {
        container.Register(typeof(IEntityService<,,>), typeof(EntityService<,,>));
        container.Register<IClientRestrictionsProvider, ClientRestrictionsProvider>();
        container.Register<IDomainService, DomainService>();
    }
}