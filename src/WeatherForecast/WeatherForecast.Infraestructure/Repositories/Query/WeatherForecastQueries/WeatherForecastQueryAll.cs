using System.Text.Json;
using WeatherForecast.Interfaces.Infraestructure.Query.WeatherForecastQueryContracts;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using WeatherForecast.Domain.Application.WeatherForecast.QueryAll;
using WeatherForecast.Infraestructure.Entities.MongoDbEntities;
using WeatherForecast.Infraestructure.MapperProfiles.WeatherForecastProfiles;
using Infraestructure.DistributedCache;

namespace WeatherForecast.Infraestructure.Repositories.Query.WeatherForecastQueries;

internal class WeatherForecastQueryAll(MongoClient mongoClient, IDistributedCleanArchitectureCache distributedCache, ILogger<WeatherForecastQueryAll> logger, WeatherForecastQueryAllMapper queryAllMapper) : IWeatherForecastQueryAllContract
{
    private readonly MongoClient _mongoClient = mongoClient;
    private readonly IDistributedCleanArchitectureCache _distributedCache = distributedCache;
    private readonly ILogger<WeatherForecastQueryAll> _logger = logger;
    private readonly WeatherForecastQueryAllMapper _queryAllMapper = queryAllMapper;

    public async Task<List<WeatherForecastQueryAllResponse>> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var weatherList = new List<WeatherForecastQueryAllResponse>();
        var cacheWeatherList = await _distributedCache.GetStringAsync("WeatherForecasts", cancellationToken);

        if (string.IsNullOrEmpty(cacheWeatherList))
        {
            var collection = _mongoClient.GetDatabase("CleanArchitecture")
                .GetCollection<WeatherForecastMongoEntity>("WeatherForecast");
            var find = await collection.FindAsync(FilterDefinition<WeatherForecastMongoEntity>.Empty, cancellationToken: cancellationToken);
            var weathers = await find.ToListAsync(cancellationToken: cancellationToken);

            weatherList = weathers.Select(_queryAllMapper.ToWeatherQueryAllResponse).ToList();
            cacheWeatherList = JsonSerializer.Serialize(weatherList);
            await _distributedCache.SetStringAsync("WeatherForecasts", cacheWeatherList, cancellationToken);
        }
        else
        {
            weatherList = JsonSerializer.Deserialize<List<WeatherForecastQueryAllResponse>>(cacheWeatherList);
        }
        _logger.LogInformation("Devolviendo los datos: {Datos}", cacheWeatherList);
        return weatherList!;
    }
}
