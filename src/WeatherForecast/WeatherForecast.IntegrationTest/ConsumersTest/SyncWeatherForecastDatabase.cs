using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using WeatherForecast.Shared.Peticiones.Request;
using WeatherForecast.Shared.Peticiones.Responses.WeatherForecast;
using Xunit;

namespace WeatherForecast.IntegrationTest;

[Collection(FixtureWeatherForecastNamesConstants.WeatherForecastTest)]
public class SyncWeatherForecastDatabase
{
    private readonly TestApiConnectionInitializer _apiConnection;

    public SyncWeatherForecastDatabase(TestApiConnectionInitializer apiConnection)
    {
        _apiConnection = apiConnection;
    }

    [Fact]
    public async Task CreateWeatherAndGetWeather_Then_CheckIfIsTheSameWeatherAndConsumerIsWorks()
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

        var weatherForecast = await client.GetAsync("WeatherForecast/all");
        var content = await weatherForecast.Content.ReadAsStringAsync();
        var weatherForecasts = JsonSerializer.Deserialize<IEnumerable<WeatherForecastResponse>>(content);
        response.Should().NotBeNull();

        weatherForecasts.Where(x => x.TemperatureC == 1
            && x.TemperatureC == 2
            && x.Summary == "Grados en Bilbao")
        .Should().NotBeNullOrEmpty();
    }
}
