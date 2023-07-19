using HostWebApi.Shared.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HostWebApi.Shared;

public static class DefaultWebApplication
{
    public static WebApplication Create(string[] args, Action<WebApplicationBuilder>? webApp = null)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddHealthChecks();

        builder.Host.AddLoggerConfiguration(builder.Configuration);
        builder.Services.AddOpenTelemetry(builder.Configuration);
        builder.Services.ConfigureDataProtectionProvider(builder.Configuration);
        builder.Services.AddControllers();

        builder.Services.AddProblemDetails();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        if (webApp is not null)
        {
            webApp.Invoke(builder);
        }

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

        app.UseHttpLogging();

        app.UseHttpsRedirection();
        app.UseAuthorization();

        app.MapControllers();

        Console.WriteLine(app.Configuration["AppName"]!);

        return app.RunAsync();
    }

}

