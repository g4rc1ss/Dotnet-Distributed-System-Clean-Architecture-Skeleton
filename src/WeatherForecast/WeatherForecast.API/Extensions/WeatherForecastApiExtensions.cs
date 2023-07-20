using WeatherForecast.Application;
using WeatherForecast.Infraestructure;

namespace WeatherForecast.API.Extensions;

public static class WeatherForecastApiExtensions
{
    public static IServiceCollection InitWeatherForecast(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(typeof(WeatherForecastApiExtensions), typeof(BusinessExtensions), typeof(WeatherForecastInfraestructureExtensions));

        services.AddBusinessServices();
        services.AddDataAccessService(configuration);

        return services;
    }
}
