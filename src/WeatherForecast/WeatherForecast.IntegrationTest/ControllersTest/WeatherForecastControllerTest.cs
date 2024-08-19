using System.Net.Http.Json;
using System.Text.Json;

using FluentAssertions;

using WeatherForecast.Shared.Peticiones.Request;
using WeatherForecast.Shared.Peticiones.Responses.WeatherForecast;

using Xunit;

namespace WeatherForecast.IntegrationTest.ControllersTest;

[Collection(FixtureWeatherForecastNamesConstants.WEATHERFORECASTTEST)]
public class WeatherForecastControllerTest(TestApiConnectionInitializer apiConnection)
{
    private readonly TestApiConnectionInitializer _apiConnection = apiConnection;

    [Fact]
    public async Task GetWeatherForecastByAPIThenReturnJsonAndDeserialiceToIEnumerableNotNullAndOneOrMoreResults()
    {

        var client = _apiConnection.WeatherForecastClient;
        var response = await client.GetAsync("WeatherForecast/all");
        var content = await response.Content.ReadAsStringAsync();
        Console.WriteLine(content);
        var weatherForecasts = JsonSerializer.Deserialize<IEnumerable<WeatherForecastResponse>>(content);
        response.Should().NotBeNull();

        foreach (var item in weatherForecasts!)
        {
            item.Should().NotBeNull();
        }
    }

    [Fact]
    public async Task CreateWeatherForecastByAPIThenReturnJsonAndDeserialiceToIEnumerableNotNullAndOneOrMoreResults()
    {

        var client = _apiConnection.WeatherForecastClient;
        var weather = new CreateWeatherForecastRequest
        {
            Celsius = 1,
            Fahrenheit = 2,
            Descripcion = "Grados en Bilbao"
        };
        var response = await client.PostAsJsonAsync("WeatherForecast/create", weather);
        Console.WriteLine(await response.Content.ReadAsStringAsync());
        response.Should().Match(x => x.IsSuccessStatusCode);
    }
}
