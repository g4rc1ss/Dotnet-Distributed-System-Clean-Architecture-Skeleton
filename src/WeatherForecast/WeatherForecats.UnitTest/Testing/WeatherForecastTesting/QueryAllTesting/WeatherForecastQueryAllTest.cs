using FluentAssertions;


using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WeatherForecats.UnitTest.Testing.WeatherForecastTesting.QueryAllTesting;

[TestClass]
public class WeatherForecastQueryAllTest
{

    [ClassInitialize]
    public static void OnInitializeTest()
    {
    }

    [TestMethod]
    public async Task GetWeatherForecastThenResponseWithOneOrMoreResults()
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
    public async Task GetWeatherForecastThenResponseWithNullValue()
    {
        var weatherForecastQuery = CasesWeatherForecastQueryAllFactory.GetFalseCaseWithCommandCreateMock;

        var response = await weatherForecastQuery.ExecuteAsync();

        response.Should().BeNull();
    }
}
