using System.Reflection;
using BuildingBlocks.EventStoreDB.Events.NoMediator;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.EventStoreDB.Core;

public static class Configuration
{
    public static IServiceCollection AddCoreServices(
        this IServiceCollection services,
        IConfiguration configuration,
        params Assembly[] assemblies
    )
    {
        var assembliesToScan = (assemblies.Any() ? assemblies : new[] { Assembly.GetEntryAssembly()! });

        return services
            .AddEventBus()
            .AddMediatR(assembliesToScan!)
            .AddEventStoreDB(configuration)
            .AddProjections(assembliesToScan);
    }
}
