using Riok.Mapperly.Abstractions;

using WeatherForecast.Domain.Application.WeatherForecast.QueryAll;
using WeatherForecast.Infraestructure.Entities.MongoDbEntities;

namespace WeatherForecast.Infraestructure.MapperProfiles.WeatherForecastProfiles;

[Mapper]
public partial class WeatherForecastQueryAllMapper
{
    [MapperIgnoreTarget(nameof(@WeatherForecastQueryAllResponse.Date))]
    public partial WeatherForecastQueryAllResponse ToWeatherQueryAllResponse(WeatherForecastMongoEntity weatherForecast);

    [MapperIgnoreTarget(nameof(@WeatherForecastMongoEntity.Date))]
    public partial WeatherForecastMongoEntity ToWeatherMongoEntity(WeatherForecastQueryAllResponse weatherForecast);
}
