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
using ServiceLayer;
using ServiceLayer.Accounts;
using ServiceLayer.Authors;
using ServiceLayer.BookDefinitions;
using ServiceLayer.BookEditions;
using ServiceLayer.Borrowing;
using ServiceLayer.CRUD;
using ServiceLayer.Domains;
using ServiceLayer.Restriction;
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

        var client = scope.GetAllEntities<Client>().First();
        var employee = scope.GetAllEntities<Employee>().First();

        var queryService = scope.GetRequiredService<IBorrowRecordQueryService>();
        var count = queryService.GetBooksLendedTodayCount(employee.Id);

        var books = scope.GetAllEntities<Book>().Take(2).Select(x => x.Id).ToIdCollection();

        var borrowService = scope.GetRequiredService<IBorrowService>();

        var success = borrowService.Borrow(client.Id, employee.Id, books);

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
    }

    private static void AddServiceLayerDependencies(Container container)
    {
        container.Register(typeof(IEntityService<>), typeof(EntityService<>));
        container.Register<IDomainQueryService, DomainQueryService>();
        container.Register<IBookEditionQueryService, BookEditionQueryService>();
        container.Register<IAccountQueryService, AccountQueryService>();
        container.Register<IClientRestrictionsProvider, ClientRestrictionsProvider>();
        container.Register<IEmployeeRestrictionsProvider, EmployeeRestrictionsProvider>();
        container.Register<IBookRestrictionsProvider, BookRestrictionsProvider>();
        container.Register<IBorrowRecordQueryService, BorrowRecordQueryService>();

        container.Register<IValidator<IdCollection>, IdCollectionValidator>();

        container.Register<IDomainService, DomainService>();
        container.Register<IValidator<Domain>, DomainValidator>();

        container.Register<IAuthorService, AuthorService>();
        container.Register<IValidator<Author>, AuthorValidator>();

        container.Register<IBookDefinitionService, BookDefinitionService>();
        container.Register<IValidator<BookDefinitionCreateOptions>, BookDefinitionCreationValidator>();
        container.Register<IValidator<BookDefinition>, BookDefinitionValidator>();

        container.Register<IBookEditionService, BookEditionService>();
        container.Register<IValidator<BookEdition>, BookEditionValidator>();
        container.Register<IValidator<BooksUpdateOptions>, BooksUpdateOptionsValidator>();

        container.Register<IAccountService, AccountService>();
        container.Register<IValidator<Account>, AccountValidator>();

        container.Register<IRestrictionsService, RestrictionsService>();
        container.Register<IBorrowService, BorrowService>();
    }
}