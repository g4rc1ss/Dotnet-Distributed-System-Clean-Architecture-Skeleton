using Application.Core;
using Infraestructure.MySqlEntityFramework;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Api
{
    public static class WebApiServicesExtension
    {
        public static IServiceCollection InicializarConfiguracionApp(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(WebApiServicesExtension), typeof(BusinessExtensions), typeof(AccessDataExtensions));
            services.AddOptions();
            services.AddRedisCache(configuration);
            services.ConfigureDataProtectionProvider(configuration);


            services.AddBusinessServices();
            services.AddDataAccessService(configuration);

            return services;
        }

        public static IServiceCollection AddRedisCache(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration["ConnectionStrings:RedisConnection"];
                options.InstanceName = "localhost";
            });
            //services.AddDistributedMemoryCache();
            return services;
        }

        public static IServiceCollection ConfigureDataProtectionProvider(this IServiceCollection services, IConfiguration configuration)
        {
            var redisConnection = ConnectionMultiplexer.Connect(configuration["ConnectionStrings:RedisConnection"]);
            var redisKeys = "DataProtection-Keys";

            services.AddDataProtection()
                //.PersistKeysToFileSystem(new DirectoryInfo(@".\temp"))
                .PersistKeysToStackExchangeRedis(redisConnection, redisKeys)
                .SetApplicationName("Aplicacion.WebApi");
            return services;
        }
    }
}
