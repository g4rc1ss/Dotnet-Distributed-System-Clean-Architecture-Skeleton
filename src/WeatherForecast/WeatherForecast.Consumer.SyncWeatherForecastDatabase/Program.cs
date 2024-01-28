using Infraestructure.Communication.Messages;
using Infraestructure.RabbitMQ;
using Infraestructure.MongoDatabase;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using HostWebApi.Shared.Extensions;
using Infraestructure.Communication.Consumers.Handler;
using OpenTelemetry.Trace;


var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices((hostBuilder, serviceCollection) =>
{
    serviceCollection.AddMongoDbConfig(hostBuilder.Configuration.GetConnectionString("MongoDbConnection")!);

    serviceCollection.AddHandlersInAssembly<Program>();
    serviceCollection.AddRabbitMQ(hostBuilder.Configuration);
    serviceCollection.AddRabbitMqConsumer<IntegrationMessage>();

    serviceCollection.AddOpenTelemetry(hostBuilder.Configuration, meter =>
    {
    }, tracer =>
    {
        tracer.AddSource(nameof(IMessageHandler));
        tracer.AddMongoDBInstrumentation();
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
