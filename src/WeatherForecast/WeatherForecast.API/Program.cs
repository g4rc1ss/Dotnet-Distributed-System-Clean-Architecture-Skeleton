using HostWebApi.Shared;
using OpenTelemetry.Trace;
using WeatherForecast.API.Extensions;

var app = DefaultWebApplication.Create(args, builder =>
{
    // Add services to the container.
    builder.Services.InitWeatherForecast(builder.Configuration);
}, metrics =>
{
}, traces =>
{
    traces.AddEntityFrameworkCoreInstrumentation();
    traces.AddMongoDBInstrumentation();
});


await DefaultWebApplication.RunAsync(app);
