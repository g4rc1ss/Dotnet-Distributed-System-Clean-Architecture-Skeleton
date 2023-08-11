using Infraestructure.Communication.Messages;
using Infraestructure.RabbitMQ;
using Infraestructure.MongoDatabase;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices((hostBuilder, serviceCollection) =>
{
    serviceCollection.AddAutoMapper(typeof(Program));
    serviceCollection.AddMongoDbConfig(hostBuilder.Configuration.GetConnectionString("MongoDbConnection")!);

    serviceCollection.AddHandlersInAssembly<Program>();
    serviceCollection.AddRabbitMQ(hostBuilder.Configuration);
    serviceCollection.AddRabbitMqConsumer<IntegrationMessage>();

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