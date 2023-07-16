using Infraestructure.MySqlDatabase.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infraestructure.MySqlDatabase;

public static class InfraestructureMySqlExtensions
{
    public static IServiceCollection AddMysqlEntityFrameworkConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMySqlDbContext<DistributedContext>(configuration);

        return services;
    }

    private static IServiceCollection AddMySqlDbContext<TContext>(this IServiceCollection services, IConfiguration configuration)
        where TContext : DbContext
    {
        void dbContextOptions(DbContextOptionsBuilder db)
        {
            var mySqlVersion = MySqlServerVersion.LatestSupportedServerVersion;
            db.UseMySql(configuration.GetConnectionString(typeof(TContext).Name), mySqlVersion);
        }

        services.AddDbContextPool<TContext>(dbContextOptions);
        services.AddPooledDbContextFactory<TContext>(dbContextOptions);

        return services;
    }
}

