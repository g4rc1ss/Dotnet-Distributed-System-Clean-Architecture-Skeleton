using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Shared.Peticiones.Request;
using Shared.Peticiones.Responses.WeatherForecast;
using TestIntegracionAPI.Initializers;
using Xunit;

namespace TestIntegracionAPI.IntegrationTest
{
    [Collection(TestCollections.WebApiTests)]
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
            var client = _apiConnection.ApiClient;
            var response = await client.GetFromJsonAsync<IEnumerable<WeatherForecastResponse>>("WeatherForecast/all");
            response.Should().NotBeNull();
            response.Should().HaveCountGreaterThan(0);

            foreach (var item in response)
            {
                item.Should().NotBeNull();
            }
        }

        [Fact]
        public async Task CreateWeatherForecastByAPI_Then_ReturnJsonAndDeserialiceToIEnumerable_NotNullAndOneOrMoreResults()
        {

            var client = _apiConnection.ApiClient;
            var weather = new CreateWeatherForecastRequest
            {
                Celsius = 1,
                Fahrenheit = 2,
                Descripcion = "Grados en Bilbao"
            };
            var response = await client.PostAsJsonAsync<CreateWeatherForecastRequest>("WeatherForecast/create", weather);
            response.Should().Match(x => x.IsSuccessStatusCode);
        }
    }
}
