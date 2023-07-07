using Application.Interfaces.ApplicationCore;
using Microsoft.Extensions.DependencyInjection;
using TestUnitarios.Mocks.MockingInfraestructure.MoqWeatherForecast.MoqCommands.MoqCreate.CommandCreateValidatingFalseData;
using TestUnitarios.Mocks.MockingInfraestructure.MoqWeatherForecast.MoqCommands.MoqCreate.CommandCreateValidatingTrueData;

namespace TestUnitarios.Testing.WeatherForecastTesting.CommandCreateTest
{
    internal class CasesWeatherForecastCreateMediatorFactory
    {
        internal static ICreateWeatherForecastContract GetTrueCaseWithCommandCreateMock => HelperTesting.CreateServiceProvider(services =>
        {
            services.AddTransient(serviceProvider => new WFCommandCreateTrueData().Mock.Object);
        }).GetRequiredService<ICreateWeatherForecastContract>();

        internal static ICreateWeatherForecastContract GetFalseCaseWithCommandCreateMock => HelperTesting.CreateServiceProvider(services =>
        {
            services.AddTransient(serviceProvider => new WFCommandCreateFalseData().Mock.Object);
        }).GetRequiredService<ICreateWeatherForecastContract>();
    }
}
