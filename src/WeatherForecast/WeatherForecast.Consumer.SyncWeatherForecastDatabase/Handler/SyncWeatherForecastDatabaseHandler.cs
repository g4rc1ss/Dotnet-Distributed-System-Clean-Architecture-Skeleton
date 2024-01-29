using Infraestructure.Communication.Consumers.Handler;
using Infraestructure.Communication.Messages;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using WeatherForecast.Infraestructure.Entities.MongoDbEntities;
using WeatherForecast.Shared.ServiceBusMessages;

namespace WeatherForecast.Consumer.SyncWeatherForecastDatabase.Handler;

public class SyncWeatherForecastDatabaseHandler : IIntegrationMessageHandler<CreateWeatherForecast>
{
    private readonly MongoClient _mongoClient;
    private readonly ILogger<SyncWeatherForecastDatabaseHandler> _logger;

    public SyncWeatherForecastDatabaseHandler(MongoClient mongoClient, ILogger<SyncWeatherForecastDatabaseHandler> logger)
    {
        _mongoClient = mongoClient;
        _logger = logger;
    }

    public async Task Handle(IntegrationMessage<CreateWeatherForecast> message, CancellationToken cancelToken = default)
    {
        var weatherForecast = GetWeatherForecastMongo(message.Content!);

        var collection = _mongoClient.GetDatabase("CleanArchitecture")
            .GetCollection<WeatherForecastMongoEntity>("WeatherForecast");

        await collection.InsertOneAsync(weatherForecast, new InsertOneOptions { }, cancelToken);
        _logger.LogInformation("Coleccion insertada en Base de datos, {cosmosDocumentId}, {mySQLDocumentId}", weatherForecast.Id, weatherForecast.MySqlId);
    }

    private WeatherForecastMongoEntity GetWeatherForecastMongo(in CreateWeatherForecast weatherForecast)
    {
        return new WeatherForecastMongoEntity
        {
            MySqlId = weatherForecast.id,
            Date = weatherForecast.date,
            Summary = weatherForecast.summary,
            TemperatureC = weatherForecast.temperatureC,
            TemperatureF = weatherForecast.temperatureF
        };
    }
}

