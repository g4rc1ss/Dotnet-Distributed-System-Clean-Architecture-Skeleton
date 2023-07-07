using System.Threading;
using Application.Interfaces.Infraestructure.Query.WeatherForecastQueryContracts;
using Moq;

namespace TestUnitarios.Mocks.MockingInfraestructure.MoqWeatherForecast.MoqQueries.QueryAll.QueryAllValidatingFalseData
{
    internal class WFQueryAllFalseData
    {
        public Mock<IWeatherForecastQueryAllContract> Mock { get; set; }

        public WFQueryAllFalseData()
        {
            Mock = new Mock<IWeatherForecastQueryAllContract>();
            Initialize();
        }

        private void Initialize()
        {
            Mock.Setup(x => x.ExecuteAsync(It.IsAny<CancellationToken>())).ReturnsAsync(FakeWFQueryAllFalseData.GetFakeWeather);
        }
    }
}
