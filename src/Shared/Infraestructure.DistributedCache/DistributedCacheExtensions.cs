using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Trace;

namespace Infraestructure.DistributedCache;

public static class DistributedCacheExtensions
{
    public static IServiceCollection AddDistributedCache(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IDistributedCleanArchitectureCache, DistributedCleanArchitectureCache>();
        services.AddStackExchangeRedisCache(redis =>
        {
            redis.Configuration = configuration.GetConnectionString("RedisConnection");
            redis.InstanceName = configuration["AppName"];
        });
        return services;
    }

    public static TracerProviderBuilder AddDistributedCacheInstrumentation(this TracerProviderBuilder tracerProvider)
    {
        return tracerProvider.AddSource(nameof(IDistributedCleanArchitectureCache));
    }
}
