using HostWebApi.Shared;
using Infraestructure.Communication.Messages;
using Infraestructure.MongoDatabase;
using Infraestructure.RabbitMQ;

var app = DefaultWebApplication.Create(args, builder =>
{
    builder.Services.AddAutoMapper(typeof(Program));
    builder.Services.AddMongoDbConfig(builder.Configuration.GetConnectionString("MongoDbConnection")!);

    builder.Services.AddHandlersInAssembly<Program>();
    builder.Services.AddRabbitMQ(builder.Configuration);
    builder.Services.AddRabbitMqConsumer<IntegrationMessage>();
});


await DefaultWebApplication.RunAsync(app);


public partial class Program { }