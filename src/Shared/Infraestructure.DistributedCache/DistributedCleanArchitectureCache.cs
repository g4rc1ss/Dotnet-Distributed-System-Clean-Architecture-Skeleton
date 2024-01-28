using System.Diagnostics;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Infraestructure.DistributedCache;

public class DistributedCleanArchitectureCache(IDistributedCache distributedCache, ILogger<DistributedCleanArchitectureCache> logger)
    : IDistributedCleanArchitectureCache
{
    private readonly ActivitySource _tracingDistributedCache = new(nameof(IDistributedCleanArchitectureCache));

    public async Task SetStringAsync(string key, string cacheWeatherList, CancellationToken cancellationToken)
    {
        using var trace = _tracingDistributedCache.StartActivity($"Set Cache");
        trace?.AddTag("cacheDataKey", key);
        logger.LogInformation("Se procede a guardar en cache la key {@cacheKey}", key);
        await distributedCache.SetStringAsync(key, cacheWeatherList, cancellationToken);
    }

    public async Task<string?> GetStringAsync(string key, CancellationToken cancellationToken)
    {
        using var trace = _tracingDistributedCache.StartActivity($"Get Cache");
        trace?.AddTag("cacheDataKey", key);
        logger.LogInformation("Se procede a obtener de cache la key {@cacheKey}", key);
        return await distributedCache.GetStringAsync(key, cancellationToken);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken)
    {
        using var trace = _tracingDistributedCache.StartActivity($"Remove Cache");
        trace?.AddTag("cacheDataKey", key);
        logger.LogInformation("Se procede a eliminar de cache la key {@cacheKey}", key);
        await distributedCache.RemoveAsync(key, cancellationToken);
    }

    public async Task RefreshAsync(string key, CancellationToken cancellationToken)
    {
        using var trace = _tracingDistributedCache.StartActivity($"Refresh Cache");
        trace?.AddTag("cacheDataKey", key);
        logger.LogInformation("Se procede refrescar de cache con key {@cacheKey}", key);

        await distributedCache.RefreshAsync(key, cancellationToken);
    }
}
