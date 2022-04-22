using BuildingBlocks.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.InternalProcessor;

public static class Extensions
{
    public static IServiceCollection AddInternalMessages(this IServiceCollection services)
    {
        services.AddHostedService<InternalMessageProcessorBackgroundService>();
        services.AddScoped<IInternalMessageService, InternalMessageService>();
        // services.AddScoped<IEventProcessor, EventProcessor>();

        return services;
    }
}
