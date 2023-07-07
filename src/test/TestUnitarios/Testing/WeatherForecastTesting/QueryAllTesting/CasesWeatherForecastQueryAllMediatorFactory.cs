using Application.Interfaces.ApplicationCore;
using Microsoft.Extensions.DependencyInjection;
using TestUnitarios.Mocks.MockingInfraestructure.MoqWeatherForecast.MoqQueries.QueryAll.QueryAllValidatingFalseData;
using TestUnitarios.Mocks.MockingInfraestructure.MoqWeatherForecast.MoqQueries.QueryAll.QueryAllValidatingTrueData;

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
