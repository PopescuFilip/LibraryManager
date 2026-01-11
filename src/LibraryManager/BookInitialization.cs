using DomainModel;
using Microsoft.Extensions.DependencyInjection;
using ServiceLayer;
using ServiceLayer.BookDefinitions;
using ServiceLayer.BookEditions;
using ServiceLayer.CRUD;
using SimpleInjector;
using System.Diagnostics.CodeAnalysis;

namespace LibraryManager;

[ExcludeFromCodeCoverage]
public static class BookInitialization
{
    public static void InitBooks(this Scope scope)
    {
        if (scope.GetAllEntities<Book>().Count != 0)
            return;

        if (scope.GetAllEntities<BookDefinition>().Count == 0)
        {
            var bookDefService = scope.GetRequiredService<IBookDefinitionService>();
            var domainService = scope.GetRequiredService<IDomainQueryService>();

            var authors = scope.GetAllEntities<Author>()
                .Take(2).Select(x => x.Id).ToIdCollection();

            var domains = new List<string>()
            {
                DomainInitialization.AlgoritmicaGrafurilor,
                DomainInitialization.AlgoritmiCuantici
            }
            .Select(domainService.GetIdByName)
            .Select(id => id ?? 0)
            .ToIdCollection();

            var bookOptions = new BookDefinitionCreateOptions("BookName", authors, domains);
            var bd = bookDefService.Create(bookOptions).Get();
        }

        if (scope.GetAllEntities<BookEdition>().Count == 0)
        {
            var bookEditionService = scope.GetRequiredService<IBookEditionService>();

            var bd = scope.GetAllEntities<BookDefinition>().First();
            bookEditionService.Create("SpecialEdition", 200, BookType.Paperback, bd.Id).Get();
        }

        if (scope.GetAllEntities<Book>().Count == 0)
        {
            var bookEditionService = scope.GetRequiredService<IBookEditionService>();

            var be = scope.GetAllEntities<BookEdition>().First();
            var options = new BooksUpdateOptions(22, 33, be.Id);

            bookEditionService.AddBooks(options).Get();
        }
    }
}