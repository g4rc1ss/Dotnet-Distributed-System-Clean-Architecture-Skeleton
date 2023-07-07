using System;
using Domain.Application.WeatherForecast.ComandCreate;
using Domain.Application.WeatherForecast.QueryAll;

namespace Application.Interfaces.ApplicationCore
{
    public interface IGetAllWeatherForecastContract
    {
        Task<List<WeatherForecastQueryAllResponse>> ExecuteAsync(CancellationToken cancellationToken = default);
    }
}

