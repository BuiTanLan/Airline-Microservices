using System.ComponentModel;
using System.Reflection;
using BuildingBlocks.EventStoreDB.Events.NoMediator;
using BuildingBlocks.EventStoreDB.OptimisticConcurrency;
using BuildingBlocks.EventStoreDB.Projections;
using BuildingBlocks.EventStoreDB.Subscriptions;
using EventStore.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.EventStoreDB;

public class EventStoreDBConfig
{
    public string ConnectionString { get; set; } = default!;
}

public record EventStoreDBOptions(
    bool UseInternalCheckpointing = true
);

public static class EventStoreDBConfigExtensions
{
    private const string DefaultConfigKey = "EventStore";

    public static IServiceCollection AddEventStoreDB(this IServiceCollection services, IConfiguration config, EventStoreDBOptions? options = null)
    {
        var eventStoreDBConfig = config.GetSection(DefaultConfigKey).Get<EventStoreDBConfig>();

        services
            .AddEventBus()
            .AddSingleton(new EventStoreClient(EventStoreClientSettings.Create(eventStoreDBConfig.ConnectionString)))
            .AddScoped<EventStoreDBExpectedStreamRevisionProvider, EventStoreDBExpectedStreamRevisionProvider>()
            .AddScoped<EventStoreDBNextStreamRevisionProvider, EventStoreDBNextStreamRevisionProvider>()
            .AddScoped<EventStoreDBOptimisticConcurrencyScope, EventStoreDBOptimisticConcurrencyScope>()
            .AddTransient<EventStoreDBSubscriptionToAll, EventStoreDBSubscriptionToAll>();

        if (options?.UseInternalCheckpointing != false)
        {
            services
                .AddTransient<ISubscriptionCheckpointRepository, EventStoreDBSubscriptionCheckpointRepository>();
        }

        return services;
    }

    public static IServiceCollection AddEventStoreDBSubscriptionToAll(
        this IServiceCollection services,
        EventStoreDBSubscriptionToAllOptions? subscriptionOptions = null,
        bool checkpointToEventStoreDB = true)
    {
        if (checkpointToEventStoreDB)
        {
            services
                .AddTransient<ISubscriptionCheckpointRepository, EventStoreDBSubscriptionCheckpointRepository>();
        }

        return services.AddHostedService(serviceProvider =>
            {
                var logger =
                    serviceProvider.GetRequiredService<ILogger<BuildingBlocks.EventStoreDB.BackgroundWorkers.BackgroundWorker>>();

                var eventStoreDBSubscriptionToAll =
                    serviceProvider.GetRequiredService<EventStoreDBSubscriptionToAll>();

                return new BuildingBlocks.EventStoreDB.BackgroundWorkers.BackgroundWorker(
                    logger,
                    ct =>
                        eventStoreDBSubscriptionToAll.SubscribeToAll(
                            subscriptionOptions ?? new EventStoreDBSubscriptionToAllOptions(),
                            ct
                        )
                );
            }
        );
    }

    public static IServiceCollection AddProjections(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddSingleton<IProjectionPublisher, ProjectionPublisher>();
        var assembliesToScan = assemblies.Any() ? assemblies : new[] { Assembly.GetEntryAssembly() };

        RegisterProjections(services, assembliesToScan!);

        return services;
    }


    private static void RegisterProjections(IServiceCollection services, Assembly[] assembliesToScan)
    {
        services.Scan(scan => scan
            .FromAssemblies(assembliesToScan)
            .AddClasses(classes => classes.AssignableTo<IProjection>()) // Filter classes
            .AsImplementedInterfaces()
            .WithTransientLifetime());
    }
}
