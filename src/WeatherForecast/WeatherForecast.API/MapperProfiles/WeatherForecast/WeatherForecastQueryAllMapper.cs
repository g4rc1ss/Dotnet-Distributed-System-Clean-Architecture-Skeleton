using Riok.Mapperly.Abstractions;

using WeatherForecast.Domain.Application.WeatherForecast.QueryAll;
using WeatherForecast.Shared.Peticiones.Responses.WeatherForecast;

namespace WeatherForecast.API.MapperProfiles.WeatherForecast;

[Mapper]
public partial class WeatherForecastQueryAllMapper
{
    public partial WeatherForecastResponse ToWeatherForecastResponse(WeatherForecastQueryAllResponse weatherForecast);
}
