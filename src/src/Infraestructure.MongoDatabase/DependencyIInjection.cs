using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Infraestructure.MongoDatabase;

public static class DependencyInjection
{

    public static IServiceCollection AddMongoDbConfig(this IServiceCollection services, IConfiguration configuration)
    {
        var host = configuration.GetConnectionString("CleanArchitectureSkeletonMongoDb");
        services.AddScoped<MongoClient>(provider => new MongoClient(host));

        return services;
    }
}

