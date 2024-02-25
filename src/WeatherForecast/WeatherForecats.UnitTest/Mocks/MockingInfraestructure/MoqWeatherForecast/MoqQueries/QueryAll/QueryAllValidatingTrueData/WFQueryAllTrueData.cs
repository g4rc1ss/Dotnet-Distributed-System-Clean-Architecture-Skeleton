using WeatherForecast.Interfaces.Infraestructure.Query.WeatherForecastQueryContracts;
using Moq;

namespace WeatherForecats.UnitTest.Mocks.MockingInfraestructure.MoqWeatherForecast.MoqQueries.QueryAll.QueryAllValidatingTrueData;

internal class WFQueryAllTrueData
{
    public Mock<IWeatherForecastQueryAllContract> Mock { get; set; }

    public WFQueryAllTrueData()
    {
        Mock = new Mock<IWeatherForecastQueryAllContract>();
        Initialize();
    }

    private void Initialize()
    {
        Mock.Setup(x => x.ExecuteAsync(It.IsAny<CancellationToken>())).ReturnsAsync(FakeWFQueryAllTrueData.GetFakeWeather);
    }
}
