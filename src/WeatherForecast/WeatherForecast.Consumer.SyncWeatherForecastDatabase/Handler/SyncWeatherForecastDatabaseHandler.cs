using Infraestructure.Communication.Consumers.Handler;
using Infraestructure.Communication.Messages;
using MongoDB.Driver;
using WeatherForecast.Infraestructure.Entities.MongoDbEntities;
using WeatherForecast.Shared.ServiceBusMessages;

namespace WeatherForecast.Consumer.SyncWeatherForecastDatabase.Handler;

public class SyncWeatherForecastDatabaseHandler : IIntegrationMessageHandler<CreateWeatherForecast>
{
    private readonly MongoClient _mongoClient;

    public SyncWeatherForecastDatabaseHandler(MongoClient mongoClient)
    {
        _mongoClient = mongoClient;
    }

    public async Task Handle(IntegrationMessage<CreateWeatherForecast> message, CancellationToken cancelToken = default)
    {
        var weatherForecast = GetWeatherForecastMongo(message.Content!);

        var collection = _mongoClient.GetDatabase("CleanArchitecture")
            .GetCollection<WeatherForecastMongoEntity>("WeatherForecast");

        await collection.InsertOneAsync(weatherForecast, new InsertOneOptions { }, cancelToken);

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

