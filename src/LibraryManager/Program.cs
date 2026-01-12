// See https://aka.ms/new-console-template for more information
using DataMapper;
using DataMapper.MigrationHelpers;
using DomainModel;
using FluentValidation;
using LibraryManager;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using NLog.Config;
using ServiceLayer;
using ServiceLayer.Accounts;
using ServiceLayer.Authors;
using ServiceLayer.BookDefinitions;
using ServiceLayer.BookEditions;
using ServiceLayer.Borrowing;
using ServiceLayer.CRUD;
using ServiceLayer.Decorators;
using ServiceLayer.Domains;
using ServiceLayer.Logging;
using ServiceLayer.Restriction;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using System.Collections.Immutable;

internal class Program
{
    private static void Main(string[] args)
    {
        var container = new Container();
        _ = GetEntryPoint(container);
        RegisterAndVerifyAll(container);
        container.Initialize();

        using var scope = AsyncScopedLifestyle.BeginScope(container);

        var entityService = scope.GetRequiredService<IEntityService<Author>>();
        var entityService2 = scope.GetRequiredService<IEntityService<Domain>>();
        var doSth = entityService.GetById(1);
        entityService2.GetAllById([]);

        var client = scope.GetAllEntities<Client>().First();
        var employee = scope.GetAllEntities<Employee>().First();

        var queryService = scope.GetRequiredService<IBorrowRecordQueryService>();
        var count = queryService.GetBooksLendedTodayCount(employee.Id);
        var countOther = queryService.GetBooksBorrowedInPeriodCount(client.Id, DateTime.Now.AddDays(-3), DateTime.Now);
        var ids = queryService.GetDomainIdsForBorrowedInPeriod(client.Id, DateTime.Now.AddDays(-3), DateTime.Now).ToList();

        var books = scope.GetAllEntities<Book>();
        var options = books.Take(1).Select(x => x.Id)
            .Select(id => new BorrowOptions(id, DateTime.Now.AddDays(7)))
            .ToImmutableArray();

        var extensionService = scope.GetRequiredService<IExtensionService>();
        var book = scope.GetAllEntities<Book>().First();
        var succes = extensionService.Extend(client.Id, book.Id, 1);

        var borrowed = scope.GetAllEntities<Book>()
            .Where(x => x.Status == BookStatus.Borrowed)
            .ToList();

        var booksQS = scope.GetRequiredService<IBookQueryService>();

        var bookDetails = booksQS.GetBookDetails(books.Select(x => x.Id).Distinct().ToIdCollection());

        var borrowService = scope.GetRequiredService<IBorrowService>();

        var repo = scope.GetRequiredService<IRepository<BorrowRecord>>();
        var borrowRecords = options.Select(s => s.BookId)
            .Select(id => new BorrowRecord(client.Id, employee.Id, id, DateTime.Now.AddDays(3)))
            .ToList();
        repo.InsertRange(borrowRecords);

        //var success = borrowService.Borrow(client.Id, employee.Id, options);

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
        container.RegisterDecorator(typeof(IEntityService<>), typeof(EntityServiceLoggingDecorator<>));
        container.Register<IDomainQueryService, DomainQueryService>();
        container.Register<IAccountQueryService, AccountQueryService>();
        container.Register<IBookEditionQueryService, BookEditionQueryService>();
        container.Register<IBookQueryService, BookQueryService>();
        container.Register<IBorrowRecordQueryService, BorrowRecordQueryService>();
        container.Register<IExtensionQueryService, ExtensionQueryService>();
        container.Register<IClientRestrictionsProvider, ClientRestrictionsProvider>();
        container.Register<IEmployeeRestrictionsProvider, EmployeeRestrictionsProvider>();
        container.Register<IBookRestrictionsProvider, BookRestrictionsProvider>();

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

        container.Register<IExtensionService, ExtensionService>();
        container.Register<IValidator<Extension>, ExtensionValidator>();

        LogManager.Configuration = new XmlLoggingConfiguration("nlog.config");
        container.Register<INLogLoggerFactory, NLogLoggerFactory>();
    }
}