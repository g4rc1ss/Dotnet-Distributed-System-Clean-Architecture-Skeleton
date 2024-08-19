using System.Net.Http.Json;

using FluentAssertions;

using WeatherForecast.Shared.Peticiones.Request;
using WeatherForecast.Shared.Peticiones.Responses.WeatherForecast;

using Xunit;

namespace WeatherForecast.IntegrationTest.ConsumersTest;

[Collection(FixtureWeatherForecastNamesConstants.WEATHERFORECASTTEST)]
public class SyncWeatherForecastDatabase(TestApiConnectionInitializer apiConnection)
{
    private readonly TestApiConnectionInitializer _apiConnection = apiConnection;

    [Fact]
    public async Task CreateWeatherAndGetWeatherThenCheckIfIsTheSameWeatherAndConsumerIsWorks()
    {
        var client = _apiConnection.WeatherForecastClient;

        var weather = new CreateWeatherForecastRequest
        {
            Celsius = 1,
            Fahrenheit = 2,
            Descripcion = "Grados en Bilbao"
        };
        var response = await client.PostAsJsonAsync("WeatherForecast/create", weather);
        response.Should().Match(x => x.IsSuccessStatusCode);

        await Task.Delay(500);

        var weatherForecast = await client.GetFromJsonAsync<IEnumerable<WeatherForecastResponse>>("WeatherForecast/all");
        response.Should().NotBeNull();

        weatherForecast.Where(x => x.TemperatureC == 1
            && x.TemperatureF == 2
            && x.Summary == "Grados en Bilbao")
        .Should().HaveCountGreaterThan(0);
    }
}
