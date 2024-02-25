using WeatherForecast.Application;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WeatherForecats.UnitTest.Mocks.MockingInfraestructure.MoqWeatherForecast;

namespace WeatherForecats.UnitTest;

internal static class HelperTesting
{
    public static IServiceProvider CreateServiceProvider(Action<IServiceCollection> addServices)
    {
        var host = new HostBuilder();

        host.ConfigureServices(services =>
        {
            services.AddSingleton(x => new MoqDataProtection().Mock.Object);
            services.AddBusinessServices();
        });

        if (addServices is not null)
        {
            host.ConfigureServices(addServices);
        }

        return host.Build().Services;
    }
}
