using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MongoDB.Driver;

namespace Infraestructure.MongoDatabase;

public static class InfraestructureMongoDbExtensions
{

    public static IServiceCollection AddMongoDbConfig(this IServiceCollection services, string connectionString)
    {
        services.AddScoped(provider => new MongoClient(connectionString));
        services.MongoDbHealthCheck(connectionString);

        return services;
    }

    private static IServiceCollection MongoDbHealthCheck(this IServiceCollection services, string connectionString)
    {
        services.AddHealthChecks()
            .AddMongoDb(connectionString, string.Empty, HealthStatus.Unhealthy, timeout: TimeSpan.FromSeconds(2));

        return services;
    }
}

