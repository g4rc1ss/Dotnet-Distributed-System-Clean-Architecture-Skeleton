using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace HostWebApi.Test.Configurations;

public class WebApplicationFactoryConfiguration : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((hostBuilder, config) =>
        {
            config.SetBasePath(Directory.GetCurrentDirectory());
        });
        builder.UseEnvironment("test");
    }
}
