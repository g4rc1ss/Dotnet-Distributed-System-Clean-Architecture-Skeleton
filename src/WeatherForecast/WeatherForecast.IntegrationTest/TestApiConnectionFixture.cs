using Xunit;

namespace WeatherForecast.IntegrationTest;

[CollectionDefinition(FixtureWeatherForecastNamesConstants.WEATHERFORECASTTEST)]
public class TestApiConnectionFixture : ICollectionFixture<TestApiConnectionInitializer>
{
}
