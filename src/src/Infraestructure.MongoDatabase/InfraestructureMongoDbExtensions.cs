using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Infraestructure.MongoDatabase;

public static class InfraestructureMongoDbExtensions
{

    public static IServiceCollection AddMongoDbConfig(this IServiceCollection services, IConfiguration configuration)
    {
        var host = configuration.GetConnectionString("CleanArchitectureSkeletonMongoDb");
        services.AddScoped(provider => new MongoClient(host));

        return services;
    }
}

