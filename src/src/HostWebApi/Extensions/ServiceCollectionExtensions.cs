using Infraestructure.MongoDatabase;
using Infraestructure.MySqlDatabase;
using Microsoft.AspNetCore.DataProtection;

namespace HostWebApi.Extensions;

public static class ServiceCollectionExtensions
{

    public static IServiceCollection AddDatabasesConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMongoDbConfig(configuration);
        services.AddMysqlEntityFrameworkConfig(configuration);

        return services;
    }

    public static IServiceCollection AddCache(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(redis =>
        {
            redis.Configuration = configuration.GetConnectionString("RedisConnection");
            redis.InstanceName = configuration["AppName"];
        });
        return services;
    }

    public static IServiceCollection ConfigureDataProtectionProvider(this IServiceCollection services, IConfiguration configuration)
    {
        var keysFolder = configuration["keysFolder"]!;
        services.AddDataProtection()
            .PersistKeysToFileSystem(new DirectoryInfo(keysFolder))
            .SetApplicationName("Aplicacion.WebApi");
        return services;
    }
}

