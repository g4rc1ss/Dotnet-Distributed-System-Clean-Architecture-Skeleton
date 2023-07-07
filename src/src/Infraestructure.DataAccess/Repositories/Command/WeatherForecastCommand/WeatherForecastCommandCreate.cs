using System.Text.Json;
using Application.Interfaces.Infraestructure.Command.WeatherForecastCommandContracts;
using AutoMapper;
using Domain.Application.WeatherForecast.ComandCreate;
using Infraestructure.MySqlDatabase.Contexts;
using Infraestructure.MySqlDatabase.DatabaseEntities;
using Infraestructure.MySqlEntityFramework.Repositories.Query.WeatherForecastQueries;
using Infraestructure.ServiceBus.SbModels;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Infraestructure.MySqlEntityFramework.Repositories.Command.WeatherForecastCommand
{
    public class WeatherForecastCommandCreate : IWeatherForecastCommandCreateContract
    {
        private readonly CleanArchitectureSkeletonContext _cleanArchitectureContext;
        private readonly IMediator _mediator;
        private readonly IDistributedCache _distributedCache;
        private readonly ILogger<WeatherForecastCommandCreate> _logger;
        private readonly IMapper _mapper;

        public WeatherForecastCommandCreate(CleanArchitectureSkeletonContext cleanArchitectureContext, IMapper mapper, IMediator mediator,
            IDistributedCache distributedCache, ILogger<WeatherForecastCommandCreate> logger)
        {
            _cleanArchitectureContext = cleanArchitectureContext;
            _mapper = mapper;
            _mediator = mediator;
            _distributedCache = distributedCache;
            _logger = logger;
        }

        public async Task<int> ExecuteAsync(WeatherForecastCommandCreateRequest weather, CancellationToken cancellationToken = default)
        {
            var weatherForecast = _mapper.Map<WeatherForecast>(weather);

            weatherForecast.Date = DateTime.Now;

            await _cleanArchitectureContext.AddAsync(weatherForecast, cancellationToken);

            var countOfSave = await _cleanArchitectureContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Guardando los datos en SQL Server: {datos}", JsonSerializer.Serialize(weatherForecast));

            if (countOfSave > 0)
            {
                await _mediator.Send<WeatherForecastCreateSbResponse>(new WeatherForecastCreateSbRequest { WeatherForecast = weatherForecast }, cancellationToken);
                await _distributedCache.RemoveAsync("WeatherForecasts", cancellationToken);
            }
            return countOfSave;
        }
    }
}
