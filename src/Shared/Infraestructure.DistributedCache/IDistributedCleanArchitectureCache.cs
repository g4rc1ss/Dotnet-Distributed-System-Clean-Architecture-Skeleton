namespace Infraestructure.DistributedCache;

public interface IDistributedCleanArchitectureCache
{
    Task<string?> GetStringAsync(string key, CancellationToken cancellationToken);
    Task RemoveAsync(string key, CancellationToken cancellationToken);
    Task SetStringAsync(string key, string cacheWeatherList, CancellationToken cancellationToken);
    Task RefreshAsync(string key, CancellationToken cancellationToken);
}
