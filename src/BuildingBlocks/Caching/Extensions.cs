using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Caching;

public static class Extensions
{
    public static IServiceCollection AddCachingRequest(this IServiceCollection services,
        IList<Assembly> assembliesToScan)
    {
        // ICacheRequest discovery and registration
        services.Scan(scan => scan
            .FromAssemblies(assembliesToScan ?? AppDomain.CurrentDomain.GetAssemblies())
            .AddClasses(classes => classes.AssignableTo(typeof(ICacheRequest<,>)),
                false)
            .AsImplementedInterfaces()
            .WithTransientLifetime());

        // IInvalidateCacheRequest discovery and registration
        services.Scan(scan => scan
            .FromAssemblies(assembliesToScan ?? AppDomain.CurrentDomain.GetAssemblies())
            .AddClasses(classes => classes.AssignableTo(typeof(IInvalidateCacheRequest<,>)),
                false)
            .AsImplementedInterfaces()
            .WithTransientLifetime());

        return services;
    }
}
