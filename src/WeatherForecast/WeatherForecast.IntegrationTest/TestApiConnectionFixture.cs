using Xunit;

namespace WeatherForecast.IntegrationTest;

[CollectionDefinition(FixtureWeatherForecastNamesConstants.WeatherForecastTest)]
public class TestApiConnectionFixture : ICollectionFixture<TestApiConnectionInitializer>
{
}
