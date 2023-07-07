using Application.Interfaces.ApplicationCore;
using Application.Interfaces.Infraestructure.Command.WeatherForecastCommandContracts;
using Domain.Application.WeatherForecast.ComandCreate;
using Microsoft.AspNetCore.DataProtection;

namespace Application.Core.UserCases.WeatherForecast
{
    internal class CreateWeatherForecast: ICreateWeatherForecastContract
    {
        private readonly IWeatherForecastCommandCreateContract _weatherForecastCommandCreate;
        private readonly IDataProtector _dataProtector;

        public CreateWeatherForecast(IWeatherForecastCommandCreateContract weatherForecastCommandCreate, IDataProtectionProvider dataProtection)
        {
            _weatherForecastCommandCreate = weatherForecastCommandCreate;
            _dataProtector = dataProtection.CreateProtector("purpose.de.creacion.Weather.Forecast");
        }

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
}
