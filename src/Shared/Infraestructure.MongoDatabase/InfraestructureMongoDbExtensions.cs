using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

using MongoDB.Driver;
using MongoDB.Driver.Core.Extensions.DiagnosticSources;

namespace Infraestructure.MongoDatabase;

public static class InfraestructureMongoDbExtensions
{

    public static IServiceCollection AddMongoDbConfig(this IServiceCollection services, string connectionString)
    {
        var clientSettings = MongoClientSettings.FromConnectionString(connectionString);
        clientSettings.ClusterConfigurator = cb => cb.Subscribe(new DiagnosticsActivityEventSubscriber());
        services.AddScoped(provider => new MongoClient(clientSettings));
        services.MongoDbHealthCheck(clientSettings);

        return services;
    }

    private static IServiceCollection MongoDbHealthCheck(this IServiceCollection services, MongoClientSettings clientSettings)
    {
        services.AddHealthChecks()
            .AddMongoDb(clientSettings, string.Empty, HealthStatus.Unhealthy, timeout: TimeSpan.FromSeconds(2));

        return services;
    }
}

