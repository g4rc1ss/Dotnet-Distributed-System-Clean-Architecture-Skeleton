using System;
using System.IO;
using System.Net.Http;
using Api;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TestIntegracionAPI.Initializers
{
    public class TestApiConnectionInitializer
    {
        public readonly string apiBaseAddress = "http://localhost:5000";
        public readonly string nameoOfHttpClient = "ApiWeather";

        public HttpClient ApiClient { get; set; }
        public IServiceProvider ServiceProvider { get; set; }
        public IConfiguration Configuration { get; set; }

        public TestApiConnectionInitializer()
        {
            var options = new WebApplicationOptions
            {
                EnvironmentName = "Development",
            };
            var builder = WebApplication.CreateBuilder(options);

            builder.Configuration.SetBasePath(Directory.GetCurrentDirectory());
            builder.Configuration.AddJsonFile("appsettings.test.json", true);

            builder.Services.AddHttpClient(nameoOfHttpClient, (client) =>
            {
                client.BaseAddress = new Uri(apiBaseAddress);
            });

            builder.Services.AddCors(option =>
            {
                option.AddPolicy("open", builder => builder.AllowAnyOrigin().AllowAnyHeader());
            });

            builder.Services.InicializarConfiguracionApp(builder.Configuration);
            builder.Services.AddProblemDetails();

            builder.Services.AddControllers()
                .AddApplicationPart(typeof(WebApiServicesExtension).Assembly);

            var app = builder.Build();

            app.UseCors("open");

            app.MapControllers();


            ServiceProvider = app.Services;
            Configuration = app.Configuration;
            ApiClient = app.Services.GetRequiredService<IHttpClientFactory>().CreateClient(nameoOfHttpClient);

            app.RunAsync(apiBaseAddress);
        }
    }
}
