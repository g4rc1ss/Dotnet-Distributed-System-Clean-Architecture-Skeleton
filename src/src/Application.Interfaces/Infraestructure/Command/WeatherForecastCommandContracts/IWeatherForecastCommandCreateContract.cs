using Domain.Application.WeatherForecast.ComandCreate;

namespace Application.Interfaces.Infraestructure.Command.WeatherForecastCommandContracts
{
    public interface IWeatherForecastCommandCreateContract
    {
        Task<int> ExecuteAsync(WeatherForecastCommandCreateRequest weather, CancellationToken cancellationToken = default);
    }
}
