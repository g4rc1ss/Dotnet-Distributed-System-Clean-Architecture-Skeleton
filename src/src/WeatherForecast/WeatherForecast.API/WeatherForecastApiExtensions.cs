using WeatherForecast.Application;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeatherForecast.Infraestructure;

namespace WeatherForecast.API;

public static class WeatherForecastApiExtensions
{
    public static IServiceCollection InitWeatherForecast(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(typeof(WeatherForecastApiExtensions), typeof(BusinessExtensions), typeof(WeatherForecastInfraestructureExtensions));
        services.AddOptions();

        services.AddBusinessServices();
        services.AddDataAccessService(configuration);

        return services;
    }
}
