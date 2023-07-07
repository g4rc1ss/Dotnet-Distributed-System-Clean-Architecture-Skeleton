using System;
using Domain.Application.WeatherForecast.ComandCreate;

namespace Application.Interfaces.ApplicationCore
{
    public interface ICreateWeatherForecastContract
    {
        Task<WeatherForecastCommandCreateResponse> ExecuteAsync(WeatherForecastCommandCreateRequest weather, CancellationToken cancellationToken = default);
    }
}

