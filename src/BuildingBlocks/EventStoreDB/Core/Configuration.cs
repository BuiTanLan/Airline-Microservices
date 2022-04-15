using System.Reflection;
using BuildingBlocks.EventStoreDB.Events.NoMediator;
using BuildingBlocks.EventStoreDB.Repository;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.EventStoreDB.Core;

public static class Configuration
{
    public static IServiceCollection AddEventStore(
        this IServiceCollection services,
        IConfiguration configuration,
        params Assembly[] assemblies
    )
    {
        var assembliesToScan = assemblies.Length > 0 ? assemblies : new[] { Assembly.GetEntryAssembly()! };

        return services
            .AddEventStoreDB(configuration)
            .AddProjections(assembliesToScan);
    }
}
