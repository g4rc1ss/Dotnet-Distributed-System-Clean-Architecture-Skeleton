using System.Text.Json;
using WeatherForecast.Interfaces.Infraestructure.Command.WeatherForecastCommandContracts;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using WeatherForecast.Domain.Application.WeatherForecast.ComandCreate;
using WeatherForecast.Infraestructure.Entities.Context;
using Infraestructure.Communication.Publisher.Integration;
using WeatherForecast.Infraestructure.MapperProfiles.WeatherForecastProfiles;
using Infraestructure.DistributedCache;

namespace WeatherForecast.Infraestructure.Repositories.Command.WeatherForecastCommand;

public class WeatherForecastCommandCreate : IWeatherForecastCommandCreateContract
{
    private DistributedContext _context;
    private readonly IDistributedCleanArchitectureCache _distributedCache;
    private readonly ILogger<WeatherForecastCommandCreate> _logger;
    private readonly IIntegrationMessagePublisher _integrationMessagePublisher;
    private readonly WeatherForecastCommandCreateMapper _weatherCreateMapper;

    public WeatherForecastCommandCreate(DistributedContext context, IDistributedCleanArchitectureCache distributedCache, ILogger<WeatherForecastCommandCreate> logger, IIntegrationMessagePublisher integrationMessagePublisher, WeatherForecastCommandCreateMapper weatherCreateMapper)
    {
        _context = context;
        _distributedCache = distributedCache;
        _logger = logger;
        _integrationMessagePublisher = integrationMessagePublisher;
        _weatherCreateMapper = weatherCreateMapper;
    }

    public async Task<int> ExecuteAsync(WeatherForecastCommandCreateRequest weather, CancellationToken cancellationToken = default)
    {
        var weatherForecast = _weatherCreateMapper.ToEfEntity(weather);
        weatherForecast.Date = DateTime.Now;

        await _context.AddAsync(weatherForecast, cancellationToken);
        var countOfSave = await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Guardando los datos en BBDD: {datos}", JsonSerializer.Serialize(weatherForecast));

        if (countOfSave > 0)
        {
            var createWeather = _weatherCreateMapper.ToWeatherForecast(weatherForecast);
            await _integrationMessagePublisher.Publish(createWeather, null, "subscription", cancellationToken);
            await _distributedCache.RemoveAsync("WeatherForecasts", cancellationToken);
        }

        return 1;
    }
}
