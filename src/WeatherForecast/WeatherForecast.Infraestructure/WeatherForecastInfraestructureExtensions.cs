using WeatherForecast.Interfaces.Infraestructure.Command.WeatherForecastCommandContracts;
using WeatherForecast.Interfaces.Infraestructure.Query.WeatherForecastQueryContracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeatherForecast.Infraestructure.Repositories.Query.WeatherForecastQueries;
using WeatherForecast.Infraestructure.Repositories.Command.WeatherForecastCommand;
using Infraestructure.MySqlDatabase;
using Infraestructure.MongoDatabase;
using WeatherForecast.Infraestructure.Entities.Context;

namespace WeatherForecast.Infraestructure;

public static class WeatherForecastInfraestructureExtensions
{
    public static IServiceCollection AddDataAccessService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRepositoryServices();

        services.AddMongoDbConfig(configuration);
        services.AddMysqlEntityFrameworkConfig<DistributedContext>(configuration);
        services.AddCache(configuration);

        return services;
    }

    private static IServiceCollection AddRepositoryServices(this IServiceCollection services)
    {
        services.AddScoped<IWeatherForecastQueryAllContract, WeatherForecastQueryAll>();
        services.AddScoped<IWeatherForecastCommandCreateContract, WeatherForecastCommandCreate>();

        return services;
    }

    private static IServiceCollection AddCache(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(redis =>
        {
            redis.Configuration = configuration.GetConnectionString("RedisConnection");
            redis.InstanceName = configuration["AppName"];
        });
        return services;
    }
}