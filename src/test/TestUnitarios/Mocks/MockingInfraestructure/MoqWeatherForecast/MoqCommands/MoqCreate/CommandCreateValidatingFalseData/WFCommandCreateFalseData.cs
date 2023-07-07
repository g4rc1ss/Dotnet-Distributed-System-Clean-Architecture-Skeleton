using System.Threading;
using Application.Interfaces.Infraestructure.Command.WeatherForecastCommandContracts;
using Domain.Application.WeatherForecast.ComandCreate;
using Moq;

namespace TestUnitarios.Mocks.MockingInfraestructure.MoqWeatherForecast.MoqCommands.MoqCreate.CommandCreateValidatingFalseData
{
    internal class WFCommandCreateFalseData
    {
        public Mock<IWeatherForecastCommandCreateContract> Mock { get; set; }

        public WFCommandCreateFalseData()
        {
            Mock = new Mock<IWeatherForecastCommandCreateContract>();
            Initialize();
        }

        private void Initialize()
        {
            Mock.Setup(x => x.ExecuteAsync(It.IsAny<WeatherForecastCommandCreateRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(0);
        }
    }
}
