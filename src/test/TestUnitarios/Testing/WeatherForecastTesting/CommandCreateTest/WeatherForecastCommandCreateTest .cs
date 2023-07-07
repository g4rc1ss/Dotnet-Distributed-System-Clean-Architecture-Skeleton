using System.Threading.Tasks;
using Domain.Application.WeatherForecast.ComandCreate;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestUnitarios.Testing.WeatherForecastTesting.CommandCreateTest;

[TestClass]
public class WeatherForecastCommandCreateTest
{

    [ClassInitialize]
    public static void OnInitializeTest(TestContext testContext)
    {
    }

    [TestMethod]
    public async Task CreateWeatherForecast_Then_ResponseWithSuccessTrue()
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
    public async Task CreateWeatherForecast_Then_ResponseWithSuccessFalse()
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
