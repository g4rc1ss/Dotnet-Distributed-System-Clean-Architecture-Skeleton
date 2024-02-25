using WeatherForecast.Interfaces.Infraestructure.Command.WeatherForecastCommandContracts;
using Moq;
using WeatherForecast.Domain.Application.WeatherForecast.ComandCreate;

namespace WeatherForecats.UnitTest.Mocks.MockingInfraestructure.MoqWeatherForecast.MoqCommands.MoqCreate.CommandCreateValidatingTrueData;

internal class WFCommandCreateTrueData
{
    public Mock<IWeatherForecastCommandCreateContract> Mock { get; set; }

    public WFCommandCreateTrueData()
    {
        Mock = new Mock<IWeatherForecastCommandCreateContract>();
        Initialize();
    }

    private void Initialize()
    {
        Mock.Setup(x => x.ExecuteAsync(It.IsAny<WeatherForecastCommandCreateRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);
    }
}
