using Infraestructure.Communication.Consumers.Handler;
using Infraestructure.Communication.Messages;

using Microsoft.Extensions.Logging;

using MongoDB.Driver;

using WeatherForecast.Infraestructure.Entities.MongoDbEntities;
using WeatherForecast.Shared.ServiceBusMessages;

namespace WeatherForecast.Consumer.SyncWeatherForecastDatabase.Handler;

public class SyncWeatherForecastDatabaseHandler(MongoClient mongoClient, ILogger<SyncWeatherForecastDatabaseHandler> logger) : IIntegrationMessageHandler<CreateWeatherForecast>
{
    private readonly MongoClient _mongoClient = mongoClient;
    private readonly ILogger<SyncWeatherForecastDatabaseHandler> _logger = logger;

    public async Task Handle(IntegrationMessage<CreateWeatherForecast> message, CancellationToken cancelToken = default)
    {
        var weatherForecast = GetWeatherForecastMongo(message.Content!);

        var collection = _mongoClient.GetDatabase("CleanArchitecture")
            .GetCollection<WeatherForecastMongoEntity>("WeatherForecast");

        await collection.InsertOneAsync(weatherForecast, new InsertOneOptions { }, cancelToken);
        _logger.LogInformation("Coleccion insertada en Base de datos, {CosmosDocumentId}, {MySQLDocumentId}", weatherForecast.Id, weatherForecast.MySqlId);
    }

    private WeatherForecastMongoEntity GetWeatherForecastMongo(in CreateWeatherForecast weatherForecast)
    {
        return new WeatherForecastMongoEntity
        {
            MySqlId = weatherForecast.Id,
            Date = weatherForecast.Date,
            Summary = weatherForecast.Summary,
            TemperatureC = weatherForecast.TemperatureC,
            TemperatureF = weatherForecast.TemperatureF
        };
    }
}

