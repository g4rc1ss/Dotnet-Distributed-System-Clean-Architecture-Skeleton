﻿using HostWebApi.Shared;
using WeatherForecast.API.Extensions;

var app = DefaultWebApplication.Create(args, builder =>
{
    // Add services to the container.
    builder.Services.InitWeatherForecast(builder.Configuration);
});


await DefaultWebApplication.RunAsync(app);

public partial class Program { }
