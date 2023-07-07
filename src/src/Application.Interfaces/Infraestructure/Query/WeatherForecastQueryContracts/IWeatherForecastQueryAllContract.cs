using Domain.Application.WeatherForecast.QueryAll;

namespace Application.Interfaces.Infraestructure.Query.WeatherForecastQueryContracts
{
    public interface IWeatherForecastQueryAllContract
    {
        Task<List<WeatherForecastQueryAllResponse>> ExecuteAsync(CancellationToken cancellationToken = default);
    }
}
