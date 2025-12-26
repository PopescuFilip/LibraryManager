// See https://aka.ms/new-console-template for more information
using DataMapper;
using DataMapper.MigrationHelpers;
using DomainModel;
using FluentValidation;
using LibraryManager;
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
        var container = new Container();
        _ = GetEntryPoint(container);
        RegisterAndVerifyAll(container);
        container.Initialize();

        var bookRecord = new Book();

        var entities = container.GetAllEntities<Domain>(
            includeProperties:
            [
                x => x.ParentDomain,
                x => x.SubDomains
            ]);

        Console.WriteLine(entities.Count);
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
        container.Register<IDomainQueryService, DomainQueryService>();

        container.Register<IValidator<Domain>, DomainValidator>();
    }

    private static void AddServiceLayerDependencies(Container container)
    {
        container.Register(typeof(IEntityService<,>), typeof(EntityService<,>));
        container.Register<IClientRestrictionsProvider, ClientRestrictionsProvider>();
        container.Register<IDomainService, DomainService>();
    }
}