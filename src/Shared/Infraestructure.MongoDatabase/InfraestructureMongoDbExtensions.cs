using Microsoft.Extensions.DependencyInjection;

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
        return services;
    }
}

