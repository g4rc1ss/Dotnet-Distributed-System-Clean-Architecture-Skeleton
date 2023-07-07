using Application.Core.UserCases.WeatherForecast;
using Application.Interfaces.ApplicationCore;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Core;

public static class BusinessExtensions
{
    public static IServiceCollection AddBusinessServices(this IServiceCollection services)
    {
        services.AddScoped<IGetAllWeatherForecastContract, GetAllWeatherForecast>();
        services.AddScoped<ICreateWeatherForecastContract, CreateWeatherForecast>();

        return services;
    }
}
