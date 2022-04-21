using BuildingBlocks.Domain;
using BuildingBlocks.InternalProcessor;
using BuildingBlocks.Outbox.EF;
using BuildingBlocks.Outbox.InMemory;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Outbox;

public static class Extensions
{
    public static IServiceCollection AddEntityFrameworkOutbox(this IServiceCollection services)
    {
        services.AddScoped<IInboxService, EfInboxService>();
        services.AddScoped<IOutboxService, EfOutboxService>();
        services.AddScoped<IInternalMessageService, InternalMessageService>();
        services.AddScoped<IEventProcessor, EventProcessor>();
        services.AddHostedService<OutboxProcessorBackgroundService>();
        services.AddHostedService<InternalMessageProcessorBackgroundService>();

        return services;
    }

    public static IServiceCollection AddInMemoryOutbox(this IServiceCollection services)
    {
        services.AddSingleton<IInMemoryOutboxStore, InMemoryOutboxStore>();
        services.AddScoped<IOutboxService, InMemoryOutboxService>();
        services.AddScoped<IInboxService, InMemoryInboxService>();
        services.AddScoped<IInternalMessageService, InternalMessageService>();
        services.AddScoped<IEventProcessor, EventProcessor>();
        services.AddHostedService<OutboxProcessorBackgroundService>();
        services.AddHostedService<InternalMessageProcessorBackgroundService>();

        return services;
    }
}
