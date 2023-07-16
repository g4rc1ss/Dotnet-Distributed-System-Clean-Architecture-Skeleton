using System.Text.Json;
using WeatherForecast.Interfaces.Infraestructure.Query.WeatherForecastQueryContracts;
using AutoMapper;
using Infraestructure.MongoDatabase.MongoDbEntities;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using WeatherForecast.Domain.Application.WeatherForecast.QueryAll;

namespace WeatherForecast.Infraestructure.Repositories.Query.WeatherForecastQueries;

internal class WeatherForecastQueryAll : IWeatherForecastQueryAllContract
{
    private readonly MongoClient _mongoClient;
    private readonly IDistributedCache _distributedCache;
    private readonly ILogger<WeatherForecastQueryAll> _logger;
    private readonly IMapper _mapper;

    public WeatherForecastQueryAll(ILogger<WeatherForecastQueryAll> logger, IMapper mapper, IDistributedCache distributedCache, MongoClient mongoClient)
    {
        _logger = logger;
        _mapper = mapper;
        _distributedCache = distributedCache;
        _mongoClient = mongoClient;
    }

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

            weatherList = _mapper.Map<List<WeatherForecastQueryAllResponse>>(weathers);
            cacheWeatherList = JsonSerializer.Serialize(weatherList);
            await _distributedCache.SetStringAsync("WeatherForecasts", cacheWeatherList, cancellationToken);
        }
        else
        {
            weatherList = JsonSerializer.Deserialize<List<WeatherForecastQueryAllResponse>>(cacheWeatherList);
        }
        _logger.LogInformation("Devolviendo los datos: {datos}", cacheWeatherList);
        return weatherList!;
    }
}
