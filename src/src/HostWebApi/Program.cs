using HostWebApi.Extensions;
using User.API;
using WeatherForecast.API;

var builder = WebApplication.CreateBuilder(args);


builder.Host.AddLoggerConfiguration(builder.Configuration);
builder.Services.AddOpenTelemetry(builder.Configuration);
builder.Services.AddDatabasesConfiguration(builder.Configuration);
builder.Services.AddCache(builder.Configuration);
builder.Services.ConfigureDataProtectionProvider(builder.Configuration);

// Add services to the container.
builder.Services.InitWeatherForecast(builder.Configuration);
builder.Services.InitUser(builder.Configuration);

builder.Services.AddProblemDetails();


builder.Services.AddControllers()
    .AddApplicationPart(typeof(WeatherForecastApiExtensions).Assembly)
    .AddApplicationPart(typeof(UserApiExtensions).Assembly);

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
