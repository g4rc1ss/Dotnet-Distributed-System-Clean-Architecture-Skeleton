using WeatherForecast.Domain.Application.WeatherForecast.QueryAll;

namespace WeatherForecast.Interfaces.ApplicationCore;

public interface IGetAllWeatherForecastContract
{
    Task<List<WeatherForecastQueryAllResponse>> ExecuteAsync(CancellationToken cancellationToken = default);
}

