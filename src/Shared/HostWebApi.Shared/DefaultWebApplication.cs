using System.Threading.RateLimiting;

using HostWebApi.Shared.Extensions;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace HostWebApi.Shared;

public static class DefaultWebApplication
{
    public static WebApplication Create(string[] args, Action<WebApplicationBuilder>? webApp = null,
        Action<MeterProviderBuilder> meterProvider = null, Action<TracerProviderBuilder> tracerProvider = null)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddHealthChecks();

        builder.Host.AddLoggerConfiguration();
        builder.Services.AddOpenTelemetry(builder.Configuration, meterProvider, tracerProvider);
        builder.Services.ConfigureDataProtectionProvider(builder.Configuration);
        builder.Services.AddControllers();
        builder.Services.AddOptions();
        builder.Services.AddRateLimiter(rateLimiter => rateLimiter.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpcontext =>
                RateLimitPartition.GetConcurrencyLimiter("Partition Key",
                _ => new ConcurrencyLimiterOptions()
                {
                    PermitLimit = Environment.ProcessorCount * 2,
                    QueueLimit = 0,
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                })));
        builder.Services.AddHttpLogging(o => { });

        builder.Services.AddProblemDetails();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        webApp?.Invoke(builder);

        return builder.Build();
    }

    public static Task RunAsync(WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (!app.Environment.IsProduction())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.MapHealthChecks("/health");

        app.UseHealthChecks("/health", new HealthCheckOptions()
        {
            Predicate = _ => true,
        });

        app.UseRateLimiter();
        app.UseHttpLogging();
        app.UseHttpsRedirection();
        app.UseAuthorization();

        app.MapControllers();

        Console.WriteLine(app.Configuration["AppName"]!);

        return app.RunAsync();
    }

}

