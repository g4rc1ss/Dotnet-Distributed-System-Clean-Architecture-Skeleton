using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;


using WeatherForecast.Domain.Application.WeatherForecast.ComandCreate;

namespace WeatherForecats.UnitTest.Testing.WeatherForecastTesting.CommandCreateTest;

[TestClass]
public class WeatherForecastCommandCreateTest
{

    [ClassInitialize]
    public static void OnInitializeTest(TestContext testContext)
    {
    }

    [TestMethod]
    public async Task CreateWeatherForecastThenResponseWithSuccessTrue()
    {
        var weatherForecastCreate = CasesWeatherForecastCreateMediatorFactory.GetTrueCaseWithCommandCreateMock;

        var response = await weatherForecastCreate.ExecuteAsync(new WeatherForecastCommandCreateRequest
        {
            Summary = "Prueba de envio de test",
            TemperatureC = 1,
            TemperatureF = 2
        });

        response.Should().NotBeNull();
        response.Success.Should().Be(true);
    }

    [TestMethod]
    public async Task CreateWeatherForecastThenResponseWithSuccessFalse()
    {
        var weatherForecastCreate = CasesWeatherForecastCreateMediatorFactory.GetFalseCaseWithCommandCreateMock;

        var response = await weatherForecastCreate.ExecuteAsync(new WeatherForecastCommandCreateRequest
        {
            Summary = "Prueba de envio de test",
            TemperatureC = 1,
            TemperatureF = 2
        });

        response.Should().NotBeNull();
        response.Success.Should().Be(false);
    }
}
