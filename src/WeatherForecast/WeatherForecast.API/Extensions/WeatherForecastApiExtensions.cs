using WeatherForecast.API.MapperProfiles.WeatherForecast;
using WeatherForecast.Application;
using WeatherForecast.Infraestructure;

namespace WeatherForecast.API.Extensions;

public static class WeatherForecastApiExtensions
{
    public static IServiceCollection InitWeatherForecast(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMapperServices();

        services.AddBusinessServices();
        services.AddDataAccessService(configuration);

        return services;
    }

    private static void AddMapperServices(this IServiceCollection services)
    {
        services.AddSingleton<WeatherForecastQueryAllMapper>();
        services.AddSingleton<WeatherForecastCommandCreateMapper>();
    }
}
