using Riok.Mapperly.Abstractions;
using WeatherForecast.Domain.Application.WeatherForecast.ComandCreate;
using WeatherForecast.Shared.Peticiones.Request;

namespace WeatherForecast.API.MapperProfiles.WeatherForecast;

[Mapper]
public partial class WeatherForecastCommandCreateMapper
{
    [MapProperty(nameof(@CreateWeatherForecastRequest.Fahrenheit), nameof(@WeatherForecastCommandCreateRequest.TemperatureF))]
    [MapProperty(nameof(@CreateWeatherForecastRequest.Celsius), nameof(@WeatherForecastCommandCreateRequest.TemperatureC))]
    [MapProperty(nameof(@CreateWeatherForecastRequest.Descripcion), nameof(@WeatherForecastCommandCreateRequest.Summary))]
    public partial WeatherForecastCommandCreateRequest ToWeatherForecastCommandRequest(CreateWeatherForecastRequest weatherForecast);
}
