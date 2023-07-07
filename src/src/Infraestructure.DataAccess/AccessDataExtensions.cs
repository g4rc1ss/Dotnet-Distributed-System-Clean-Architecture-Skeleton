using Application.Interfaces.Infraestructure.Command.WeatherForecastCommandContracts;
using Application.Interfaces.Infraestructure.Query.WeatherForecastQueryContracts;
using Infraestructure.MongoDatabase;
using Infraestructure.MySqlDatabase;
using Infraestructure.MySqlEntityFramework.Repositories.Command.WeatherForecastCommand;
using Infraestructure.MySqlEntityFramework.Repositories.Query.WeatherForecastQueries;
using Infraestructure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infraestructure.MySqlEntityFramework;

public static class AccessDataExtensions
{
    public static IServiceCollection AddDataAccessService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMongoDbConfig(configuration);
        services.AddMysqlEntityFrameworkConfig(configuration);
        services.AddServiceBusConfig();

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
