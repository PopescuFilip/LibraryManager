// See https://aka.ms/new-console-template for more information
using DataMapper;
using DataMapper.MigrationHelpers;
using DomainModel;
using FluentValidation;
using LibraryManager;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ServiceLayer.Authors;
using ServiceLayer.BookDefinitions;
using ServiceLayer.CRUD;
using ServiceLayer.Domains;
using SimpleInjector;
using SimpleInjector.Lifestyles;

internal class Program
{
    private static void Main(string[] args)
    {
        var container = new Container();
        _ = GetEntryPoint(container);
        RegisterAndVerifyAll(container);
        container.Initialize();

        using var scope = AsyncScopedLifestyle.BeginScope(container);

        var authorIds = scope.GetAllEntities<Author>()
            .Take(2)
            .Select(a => a.Id)
            .ToList();

        var domains = new List<string>()
        {
            DomainInitialization.AlgoritmiCuantici,
            DomainInitialization.AlgoritmicaGrafurilor
        };

        var queryService = scope.GetRequiredService<IDomainQueryService>();

        var domainIds = domains
            .Select(queryService.GetIdByName)
            .Select(x => x ?? 0)
            .ToList() ?? [];

        var bookService = scope.GetRequiredService<IBookDefinitionService>();

        var options = new BookDefinitionCreateOptions(
            "bookName23",
            [.. authorIds],
            [.. domainIds]);

        var createdBook = bookService.Create(options).Get();

        Console.WriteLine("Hello world!");
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
            .AddDbContext<LibraryDbContext>(options => options.Configure(context.Configuration));
        })
        .ConfigureAppConfiguration(builder =>
        {
            builder.AddConfiguration(AppSettings.Configuration);
        })
        .Build()
        .UseSimpleInjector(container);

    private static void AddDataMapperDependencies(Container container)
    {
        container.Register(typeof(IRepository<>), typeof(Repository<>));
        container.Register<IRestrictionsProvider, RestrictionsProvider>();

        container.Register<IDomainQueryService, DomainQueryService>();
    }

    private static void AddServiceLayerDependencies(Container container)
    {
        container.Register(typeof(IEntityService<>), typeof(EntityService<>));
        container.Register<IClientRestrictionsProvider, ClientRestrictionsProvider>();
        container.Register<IBookRestrictionsProvider, BookRestrictionsProvider>();

        container.Register<IDomainService, DomainService>();
        container.Register<IValidator<Domain>, DomainValidator>();

        container.Register<IAuthorService, AuthorService>();
        container.Register<IValidator<Author>, AuthorValidator>();

        container.Register<IBookDefinitionService, BookDefinitionService>();
        container.Register<IValidator<BookDefinitionCreateOptions>, BookDefinitionCreationValidator>();
        container.Register<IValidator<BookDefinition>, BookDefinitionValidator>();
    }
}