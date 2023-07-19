using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestUnitarios.Testing.WeatherForecastTesting.QueryAllTesting;

[TestClass]
public class WeatherForecastQueryAllTest
{

    [ClassInitialize]
    public static void OnInitializeTest(TestContext testContext)
    {
    }

    [TestMethod]
    public async Task GetWeatherForecast_Then_ResponseWithOneOrMoreResults()
    {
        var weatherForecastQuery = CasesWeatherForecastQueryAllFactory.GetTrueCaseWithCommandCreateMock;

        var response = await weatherForecastQuery.ExecuteAsync();

        response.Should().HaveCount(10);
        foreach (var item in response)
        {
            item.Should().NotBeNull();
            item.TemperatureC.Should().BeOfType(typeof(int));
        }
    }

    [TestMethod]
    public async Task GetWeatherForecast_Then_ResponseWithNullValue()
    {
        var weatherForecastQuery = CasesWeatherForecastQueryAllFactory.GetFalseCaseWithCommandCreateMock;

        var response = await weatherForecastQuery.ExecuteAsync();

        response.Should().BeNull();
    }
}
