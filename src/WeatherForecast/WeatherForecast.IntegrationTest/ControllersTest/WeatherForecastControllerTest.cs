using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using WeatherForecast.Shared.Peticiones.Request;
using WeatherForecast.Shared.Peticiones.Responses.WeatherForecast;
using Xunit;

namespace WeatherForecast.IntegrationTest.ControllersTest;

[Collection(FixtureWeatherForecastNamesConstants.WeatherForecastTest)]
public class WeatherForecastControllerTest
{
    private readonly TestApiConnectionInitializer _apiConnection;

    public WeatherForecastControllerTest(TestApiConnectionInitializer apiConnection)
    {
        _apiConnection = apiConnection;
    }

    [Fact]
    public async Task GetWeatherForecastByAPI_Then_ReturnJsonAndDeserialiceToIEnumerable_NotNullAndOneOrMoreResults()
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
    public async Task CreateWeatherForecastByAPI_Then_ReturnJsonAndDeserialiceToIEnumerable_NotNullAndOneOrMoreResults()
    {

        var client = _apiConnection.WeatherForecastClient;
        var weather = new CreateWeatherForecastRequest
        {
            Celsius = 1,
            Fahrenheit = 2,
            Descripcion = "Grados en Bilbao"
        };
        var response = await client.PostAsJsonAsync<CreateWeatherForecastRequest>("WeatherForecast/create", weather);
        Console.WriteLine(await response.Content.ReadAsStringAsync());
        response.Should().Match(x => x.IsSuccessStatusCode);
    }
}
