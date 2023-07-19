using WeatherForecast.Interfaces.ApplicationCore;
using Microsoft.Extensions.DependencyInjection;
using WeatherForecats.UnitTest.Mocks.MockingInfraestructure.MoqWeatherForecast.MoqQueries.QueryAll.QueryAllValidatingTrueData;
using WeatherForecats.UnitTest.Mocks.MockingInfraestructure.MoqWeatherForecast.MoqQueries.QueryAll.QueryAllValidatingFalseData;

namespace TestUnitarios.Testing.WeatherForecastTesting.QueryAllTesting
{
    internal class CasesWeatherForecastQueryAllFactory
    {
        internal static IGetAllWeatherForecastContract GetTrueCaseWithCommandCreateMock => HelperTesting.CreateServiceProvider(services =>
        {
            services.AddTransient(serviceProvider => new WFQueryAllTrueData().Mock.Object);
        }).GetRequiredService<IGetAllWeatherForecastContract>();

        internal static IGetAllWeatherForecastContract GetFalseCaseWithCommandCreateMock => HelperTesting.CreateServiceProvider(services =>
        {
            services.AddTransient(serviceProvider => new WFQueryAllFalseData().Mock.Object);
        }).GetRequiredService<IGetAllWeatherForecastContract>();
    }
}
