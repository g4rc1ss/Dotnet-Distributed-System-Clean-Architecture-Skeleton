using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infraestructure.MySqlDatabase;

public static class InfraestructureMySqlExtensions
{
    public static IServiceCollection AddMysqlEntityFrameworkConfig<TContext>(this IServiceCollection services, IConfiguration configuration)
        where TContext : DbContext
    {
        services.AddMySqlDbContext<TContext>(configuration);
        services.AddMySqlHealthCheck<TContext>();

        return services;
    }

    private static IServiceCollection AddMySqlDbContext<TContext>(this IServiceCollection services, IConfiguration configuration)
        where TContext : DbContext
    {
        void dbContextOptions(DbContextOptionsBuilder db)
        {
            var connectionString = configuration.GetConnectionString(typeof(TContext).Name);
            var mySqlVersion = ServerVersion.AutoDetect(connectionString);
            db.UseMySql(connectionString, mySqlVersion);
        }

        services.AddDbContextPool<TContext>(dbContextOptions);
        services.AddPooledDbContextFactory<TContext>(dbContextOptions);

        return services;
    }

    private static IServiceCollection AddMySqlHealthCheck<TContext>(this IServiceCollection services)
        where TContext : DbContext
    {
        services.AddHealthChecks()
            .AddDbContextCheck<TContext>();

        return services;
    }
}

