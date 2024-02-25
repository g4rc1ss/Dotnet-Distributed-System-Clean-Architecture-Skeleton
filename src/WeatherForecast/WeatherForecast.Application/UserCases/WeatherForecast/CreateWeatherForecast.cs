using WeatherForecast.Interfaces.Infraestructure.Command.WeatherForecastCommandContracts;
using Microsoft.AspNetCore.DataProtection;
using WeatherForecast.Domain.Application.WeatherForecast.ComandCreate;
using WeatherForecast.Interfaces.ApplicationCore;

namespace WeatherForecast.Application.UserCases.WeatherForecast;

internal class CreateWeatherForecast(IWeatherForecastCommandCreateContract weatherForecastCommandCreate, IDataProtectionProvider dataProtection) : ICreateWeatherForecastContract
{
    private readonly IWeatherForecastCommandCreateContract _weatherForecastCommandCreate = weatherForecastCommandCreate;
    private readonly IDataProtector _dataProtector = dataProtection.CreateProtector("purpose.de.creacion.Weather.Forecast");

    public async Task<WeatherForecastCommandCreateResponse> ExecuteAsync(WeatherForecastCommandCreateRequest weather, CancellationToken cancellationToken = default)
    {
        ProtectFieldsToSave(ref weather);
        var createWeatherForecast = await _weatherForecastCommandCreate.ExecuteAsync(weather, cancellationToken);

        return new WeatherForecastCommandCreateResponse
        {
            Success = createWeatherForecast > 0,
        };
    }

    private void ProtectFieldsToSave(ref WeatherForecastCommandCreateRequest weatherForecast)
    {
        weatherForecast.Summary = _dataProtector.Protect(weatherForecast.Summary!);
    }
}
