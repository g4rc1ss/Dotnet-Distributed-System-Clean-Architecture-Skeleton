using Infraestructure.MySqlDatabase.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infraestructure.MySqlDatabase;

public static class DependencyInjection
{
    public static IServiceCollection AddMysqlEntityFrameworkConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMySqlDbContext<CleanArchitectureSkeletonContext>(configuration);

        return services;
    }

    private static IServiceCollection AddMySqlDbContext<TContext>(this IServiceCollection services, IConfiguration configuration) where TContext : DbContext
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

