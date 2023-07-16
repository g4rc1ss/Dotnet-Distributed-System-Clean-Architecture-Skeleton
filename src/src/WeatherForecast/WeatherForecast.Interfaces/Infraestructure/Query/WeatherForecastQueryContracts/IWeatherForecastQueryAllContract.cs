using WeatherForecast.Domain.Application.WeatherForecast.QueryAll;

namespace WeatherForecast.Interfaces.Infraestructure.Query.WeatherForecastQueryContracts;

public interface IWeatherForecastQueryAllContract
{
    Task<List<WeatherForecastQueryAllResponse>> ExecuteAsync(CancellationToken cancellationToken = default);
}
