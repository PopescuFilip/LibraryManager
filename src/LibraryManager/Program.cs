// See https://aka.ms/new-console-template for more information
using DataMapper;
using DataMapper.MigrationHelpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServiceLayer;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        var entryPoint = GetEntryPoint();

        var provider = entryPoint.Services.GetRequiredService<IClientRestrictionsProvider>();

        var first = provider.GetClientRestrictions();
        var second = provider.GetPrivilegedClientRestrictions();
        Console.WriteLine(first);
        Console.WriteLine();
        Console.WriteLine(second);
    }

    private static IHost GetEntryPoint() =>
        Host
        .CreateDefaultBuilder()
        .ConfigureServices((context, services) =>
        {
            AddDataMapperDependencies(services);
            AddServiceLayerDependencies(services);
        })
        .ConfigureAppConfiguration(builder =>
        {
            builder.AddConfiguration(AppSettings.Configuration);
        })
        .Build();

    private static IServiceCollection AddDataMapperDependencies(IServiceCollection services) =>
        services
        .AddDbContextFactory<LibraryDbContext, LibraryDbContextFactory>()
        .AddScoped<IRestrictionsProvider, RestrictionsProvider>();

    private static IServiceCollection AddServiceLayerDependencies(IServiceCollection services) =>
        services
        .AddScoped<IClientRestrictionsProvider, ClientRestrictionsProvider>();
}