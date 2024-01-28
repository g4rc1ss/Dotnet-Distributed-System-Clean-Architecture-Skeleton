﻿using Infraestructure.Communication.Messages;
using Infraestructure.RabbitMQ;
using Infraestructure.MongoDatabase;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Infraestructure.Communication.Consumers;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;


var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices((hostBuilder, serviceCollection) =>
{
    serviceCollection.AddAutoMapper(typeof(Program));
    serviceCollection.AddMongoDbConfig(hostBuilder.Configuration.GetConnectionString("MongoDbConnection")!);

    serviceCollection.AddHandlersInAssembly<Program>();
    serviceCollection.AddRabbitMQ(hostBuilder.Configuration);
    serviceCollection.AddRabbitMqConsumer<IntegrationMessage>();

    serviceCollection.AddOpenTelemetry()
        .ConfigureResource(resource =>
        {
            resource.AddService("Consumer Weather Forecast");
        }).WithTracing(traces =>
        {
            traces.AddMongoDBInstrumentation();
            traces.AddSource(nameof(IMessageConsumer));
            traces.AddOtlpExporter(exporter =>
            {
                exporter.Endpoint = new Uri(hostBuilder.Configuration["ConnectionStrings:OpenTelemetry"]!);
            });
        });
});

builder.ConfigureAppConfiguration(configuration =>
{

});

var app = builder.Build();

var environment = app.Services.GetRequiredService<IHostEnvironment>();
var config = app.Services.GetRequiredService<IConfiguration>();

Console.WriteLine($"{environment.EnvironmentName}");
Console.WriteLine(config["AppName"]);

await app.RunAsync();
