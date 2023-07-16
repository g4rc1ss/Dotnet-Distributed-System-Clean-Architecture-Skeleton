using WeatherForecast.Domain.Application.WeatherForecast.ComandCreate;

namespace WeatherForecast.Interfaces.ApplicationCore;

public interface ICreateWeatherForecastContract
{
    Task<WeatherForecastCommandCreateResponse> ExecuteAsync(WeatherForecastCommandCreateRequest weather, CancellationToken cancellationToken = default);
}

