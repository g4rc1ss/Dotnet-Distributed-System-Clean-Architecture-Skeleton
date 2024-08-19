using WeatherForecast.Interfaces.ApplicationCore;
using Microsoft.Extensions.DependencyInjection;
using WeatherForecast.Application.UserCases.WeatherForecast;

namespace WeatherForecast.Application;

public static class BusinessExtensions
{
    public static IServiceCollection AddBusinessServices(this IServiceCollection services)
    {
        services.AddScoped<IGetAllWeatherForecastContract, GetAllWeatherForecast>();
        services.AddScoped<ICreateWeatherForecastContract, CreateWeatherForecast>();

        return services;
    }
}
