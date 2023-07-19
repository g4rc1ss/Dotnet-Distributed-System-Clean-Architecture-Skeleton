using WeatherForecast.Domain.Application.WeatherForecast.ComandCreate;

namespace WeatherForecast.Interfaces.Infraestructure.Command.WeatherForecastCommandContracts;

public interface IWeatherForecastCommandCreateContract
{
    Task<int> ExecuteAsync(WeatherForecastCommandCreateRequest weather, CancellationToken cancellationToken = default);
}
