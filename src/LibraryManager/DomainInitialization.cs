using DomainModel;
using Microsoft.Extensions.DependencyInjection;
using ServiceLayer.Domains;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace LibraryManager;

public static class DomainInitialization
{
    public const string Stiinta = "Stiinta";

    public const string Matematica = "Matematica";
    public const string Fizica = "Fizica";
    public const string Chimie = "Chimie";
    public const string Informatica = "Informatica";

    public const string Algoritmi = "Algoritmi";
    public const string Programare = "Programare";
    public const string BazeDeDate = "Baze de date";
    public const string ReteleDeCalculatoare = "Retele de calculatoare";

    public const string AlgoritmicaGrafurilor = "Algoritmica grafurilor";
    public const string AlgoritmiCuantici = "Algoritmi cuantici";

    public static void InitDomains(this Container container)
    {
        using var scope = AsyncScopedLifestyle.BeginScope(container);
        var domainService = scope.GetRequiredService<IDomainService>();

        if (scope.GetAllEntities<Domain>().Count != 0)
            return;

        domainService.Add(Stiinta);

        domainService.Add(Matematica, Stiinta);
        domainService.Add(Fizica, Stiinta);
        domainService.Add(Chimie, Stiinta);
        domainService.Add(Informatica, Stiinta);

        domainService.Add(Algoritmi, Informatica);
        domainService.Add(Programare, Informatica);
        domainService.Add(BazeDeDate, Informatica);
        domainService.Add(ReteleDeCalculatoare, Informatica);

        domainService.Add(AlgoritmicaGrafurilor, Algoritmi);
        domainService.Add(AlgoritmiCuantici, Algoritmi);
    }
}