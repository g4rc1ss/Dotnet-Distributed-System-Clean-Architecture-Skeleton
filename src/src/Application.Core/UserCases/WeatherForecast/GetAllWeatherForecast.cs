using Application.Interfaces.ApplicationCore;
using Application.Interfaces.Infraestructure.Query.WeatherForecastQueryContracts;
using Domain.Application.WeatherForecast.ComandCreate;
using Domain.Application.WeatherForecast.QueryAll;
using Microsoft.AspNetCore.DataProtection;

namespace Application.Core.UserCases.WeatherForecast;

internal class GetAllWeatherForecast : IGetAllWeatherForecastContract
{
    private readonly IWeatherForecastQueryAllContract _weatherForecastQueryAll;
    private readonly IDataProtector _dataPotector;

    public GetAllWeatherForecast(IWeatherForecastQueryAllContract weatherForecastQueryAll, IDataProtectionProvider dataProtectionProvider)
    {
        _weatherForecastQueryAll = weatherForecastQueryAll;
        _dataPotector = dataProtectionProvider.CreateProtector("purpose.de.creacion.Weather.Forecast");
    }

    public async Task<List<WeatherForecastQueryAllResponse>> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var weathers = await _weatherForecastQueryAll.ExecuteAsync(cancellationToken);
        if (weathers is not null)
        {
            return UnProtectData(ref weathers);
        }

        return new();
    }


    private List<WeatherForecastQueryAllResponse> UnProtectData(ref List<WeatherForecastQueryAllResponse> weatherForecasts)
    {
        return weatherForecasts.Select(x => new WeatherForecastQueryAllResponse
        {
            Date = x.Date,
            Summary = _dataPotector.Unprotect(x.Summary!),
            TemperatureC = x.TemperatureC,
            TemperatureF = x.TemperatureF,
        }).ToList();
    }
}
