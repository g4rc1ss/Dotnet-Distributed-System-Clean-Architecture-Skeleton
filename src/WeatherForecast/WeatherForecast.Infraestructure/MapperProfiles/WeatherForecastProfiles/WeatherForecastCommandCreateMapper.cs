using Riok.Mapperly.Abstractions;

using WeatherForecast.Domain.Application.WeatherForecast.ComandCreate;
using WeatherForecast.Infraestructure.Entities.DbEntities;
using WeatherForecast.Infraestructure.Entities.MongoDbEntities;
using WeatherForecast.Shared.ServiceBusMessages;

namespace WeatherForecast.Infraestructure.MapperProfiles.WeatherForecastProfiles;

[Mapper]
public partial class WeatherForecastCommandCreateMapper
{
    public partial WeatherForecastMongoEntity ToMongoEntity(WeatherForecastCommandCreateRequest weatherForecastRequest);
    public partial WeatherForecastEfEntity ToEfEntity(WeatherForecastCommandCreateRequest weatherForecastRequest);

    [MapProperty(nameof(@WeatherForecastEfEntity.Id), nameof(@CreateWeatherForecast.Id))]
    [MapProperty(nameof(@WeatherForecastEfEntity.Summary), nameof(@CreateWeatherForecast.Summary))]
    [MapProperty(nameof(@WeatherForecastEfEntity.TemperatureF), nameof(@CreateWeatherForecast.TemperatureF))]
    [MapProperty(nameof(@WeatherForecastEfEntity.TemperatureC), nameof(@CreateWeatherForecast.TemperatureC))]
    public partial CreateWeatherForecast ToWeatherForecast(WeatherForecastEfEntity weatherForecastEf);
}
