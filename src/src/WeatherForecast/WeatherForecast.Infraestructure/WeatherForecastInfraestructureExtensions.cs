using WeatherForecast.Interfaces.Infraestructure.Command.WeatherForecastCommandContracts;
using WeatherForecast.Interfaces.Infraestructure.Query.WeatherForecastQueryContracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeatherForecast.Infraestructure.Repositories.Query.WeatherForecastQueries;
using WeatherForecast.Infraestructure.Repositories.Command.WeatherForecastCommand;

namespace WeatherForecast.Infraestructure;

public static class WeatherForecastInfraestructureExtensions
{
    public static IServiceCollection AddDataAccessService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRepositoryServices();

        return services;
    }

    private static IServiceCollection AddRepositoryServices(this IServiceCollection services)
    {
        services.AddScoped<IWeatherForecastQueryAllContract, WeatherForecastQueryAll>();
        services.AddScoped<IWeatherForecastCommandCreateContract, WeatherForecastCommandCreate>();

        return services;
    }
}