using HostWebApi.Test.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WeatherForecast.IntegrationTest;

public class TestApiConnectionInitializer
{
    public HttpClient ApiClient { get; set; }
    public IServiceProvider ServiceProvider { get; set; }
    public IConfiguration Configuration { get; set; }

    public TestApiConnectionInitializer()
    {
        try
        {
            var webHost = new WebApplicationFactoryConfiguration();
            ServiceProvider = webHost.Services;
            ApiClient = webHost.CreateClient();
            Configuration = ServiceProvider.GetRequiredService<IConfiguration>();
        }
        finally
        {

        }
    }
}