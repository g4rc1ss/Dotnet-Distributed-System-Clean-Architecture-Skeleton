using System.Text.Json;
using WeatherForecast.Interfaces.Infraestructure.Command.WeatherForecastCommandContracts;
using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using WeatherForecast.Domain.Application.WeatherForecast.ComandCreate;
using WeatherForecast.Infraestructure.Entities.Context;
using WeatherForecast.Infraestructure.Entities.DbEntities;
using Infraestructure.Communication.Publisher.Integration;

namespace WeatherForecast.Infraestructure.Repositories.Command.WeatherForecastCommand;

public class WeatherForecastCommandCreate : IWeatherForecastCommandCreateContract
{
    private DistributedContext _context;
    private readonly IDistributedCache _distributedCache;
    private readonly ILogger<WeatherForecastCommandCreate> _logger;
    private readonly IIntegrationMessagePublisher _integrationMessagePublisher;
    private readonly IMapper _mapper;

    public WeatherForecastCommandCreate(DistributedContext context, IDistributedCache distributedCache, ILogger<WeatherForecastCommandCreate> logger, IMapper mapper, IIntegrationMessagePublisher integrationMessagePublisher)
    {
        _context = context;
        _distributedCache = distributedCache;
        _logger = logger;
        _mapper = mapper;
        _integrationMessagePublisher = integrationMessagePublisher;
    }

    public async Task<int> ExecuteAsync(WeatherForecastCommandCreateRequest weather, CancellationToken cancellationToken = default)
    {
        var weatherForecast = _mapper.Map<WeatherForecastEfEntity>(weather);

        weatherForecast.Date = DateTime.Now;

        await _context.AddAsync(weatherForecast, cancellationToken);

        var countOfSave = await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Guardando los datos en BBDD: {datos}", JsonSerializer.Serialize(weatherForecast));

        if (countOfSave > 0)
        {
            await _integrationMessagePublisher.Publish(weatherForecast, null, "subscription", cancellationToken);
            await _distributedCache.RemoveAsync("WeatherForecasts", cancellationToken);
        }

        return 1;
    }
}
