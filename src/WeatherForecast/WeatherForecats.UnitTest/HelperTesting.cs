using System;
using WeatherForecast.Application;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WeatherForecats.UnitTest.Mocks.MockingInfraestructure.MoqWeatherForecast;

namespace TestUnitarios
{
    internal static class HelperTesting
    {
        public static IServiceProvider CreateServiceProvider(Action<IServiceCollection> addServices)
        {
            var host = new HostBuilder();

            host.ConfigureServices(services =>
            {
                services.AddSingleton<IDataProtectionProvider>(x => new MoqDataProtection().Mock.Object);
                services.AddBusinessServices();
            });

            if (addServices is not null)
            {
                host.ConfigureServices(addServices);
            }

            return host.Build().Services;
        }
    }
}
