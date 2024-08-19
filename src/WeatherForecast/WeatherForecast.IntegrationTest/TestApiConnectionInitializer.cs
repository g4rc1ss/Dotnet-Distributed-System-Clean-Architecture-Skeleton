using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WeatherForecast.IntegrationTest;

public class TestApiConnectionInitializer
{
    public HttpClient WeatherForecastClient { get; set; }
    public HttpClient UsersClient { get; set; }
    public IServiceProvider ServiceProvider { get; set; }
    public IConfiguration Configuration { get; set; }

    public TestApiConnectionInitializer()
    {
        try
        {
            ServiceProvider = HelperTesting.ConfigureServices();
            Configuration = ServiceProvider.GetRequiredService<IConfiguration>();
            WeatherForecastClient = ServiceProvider.GetRequiredService<IHttpClientFactory>().CreateClient("weatherForecastapi");
            UsersClient = ServiceProvider.GetRequiredService<IHttpClientFactory>().CreateClient("usersapi");
        }
        finally
        {

        }
    }
}
