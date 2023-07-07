using Api;
using HostWebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddLoggerConfiguration(builder.Configuration);
builder.Services.AddOpenTelemetry(builder.Configuration);

// Add services to the container.
builder.Services.InicializarConfiguracionApp(builder.Configuration);
builder.Services.AddProblemDetails();


builder.Services.AddControllers()
    .AddApplicationPart(typeof(WebApiServicesExtension).Assembly);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpLogging();

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

Console.WriteLine(app.Configuration["AppName"]!);

await app.RunAsync();


public partial class Program { }
