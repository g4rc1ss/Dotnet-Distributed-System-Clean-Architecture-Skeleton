using System.Text.Json;
using AutoMapper;
using Infraestructure.MongoDatabase.MongoDbEntities;
using Infraestructure.ServiceBus.SbModels;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Infraestructure.ServiceBus.SbCommand.WeatherForecastSbCommand
{
    public class WeatherForecastCreateCommandHandler : IRequestHandler<WeatherForecastCreateSbRequest, WeatherForecastCreateSbResponse>
    {
        private readonly MongoClient _mongoClient;
        private readonly IMapper _mapper;
        private readonly ILogger<WeatherForecastCreateCommandHandler> _logger;

        public WeatherForecastCreateCommandHandler(MongoClient mongoClient, IMapper mapper, ILogger<WeatherForecastCreateCommandHandler> logger)
        {
            _mongoClient = mongoClient;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<WeatherForecastCreateSbResponse> Handle(WeatherForecastCreateSbRequest request, CancellationToken cancellationToken)
        {
            var weather = _mapper.Map<WeatherForecast>(request.WeatherForecast);
            var collection = _mongoClient.GetDatabase("CleanArchitecture")
                .GetCollection<WeatherForecast>("WeatherForecast");

            await collection.InsertOneAsync(weather, new InsertOneOptions { }, cancellationToken);
            _logger.LogInformation("Guardando los datos en Mongo: {datos}", JsonSerializer.Serialize(weather));

            return new WeatherForecastCreateSbResponse
            {
                Succeeded = true,
            };
        }
    }
}

