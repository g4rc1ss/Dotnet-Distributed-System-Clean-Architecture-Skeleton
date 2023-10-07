using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WeatherForecast.IntegrationTest;

public static class HelperTesting
{
    public static IServiceProvider ConfigureServices()
    {
        var host = new HostBuilder();

        host.ConfigureAppConfiguration((hostBuilder, config) =>
        {
            config.SetBasePath(Directory.GetCurrentDirectory());
            config.AddJsonFile("appsettings.test.json");
        });

        host.ConfigureServices(services =>
        {
            services.AddHttpClient("weatherForecastapi", (serviceProvider, client) =>
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                var usersWeatherForecast = configuration["weatherForecastapi"];
                client.BaseAddress = new Uri(usersWeatherForecast!);
            }).ConfigurePrimaryHttpMessageHandler(() =>
            {
                var handler = new HttpClientHandler
                {
                    ClientCertificateOptions = ClientCertificateOption.Manual,
                    ServerCertificateCustomValidationCallback = (httpRequest, cert, cetChain, policyErrors) => true
                };

                return handler;
            });

            services.AddHttpClient("usersapi", (serviceProvider, client) =>
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                var usersapiBase = configuration["usersapi"];
                client.BaseAddress = new Uri(usersapiBase!);
            }).ConfigurePrimaryHttpMessageHandler(() =>
            {
                var handler = new HttpClientHandler
                {
                    ClientCertificateOptions = ClientCertificateOption.Manual,
                    ServerCertificateCustomValidationCallback = (httpRequest, cert, cetChain, policyErrors) => true
                };

                return handler;
            });
        });

        return host.Build().Services;
    }
}
