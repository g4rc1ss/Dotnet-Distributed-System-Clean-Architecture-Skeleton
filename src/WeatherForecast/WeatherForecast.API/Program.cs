﻿using HostWebApi.Shared;
using Infraestructure.Communication.Messages;
using Infraestructure.Communication.Publisher;
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
    traces.AddSource(nameof(IExternalMessagePublisher<IMessage>));
    traces.AddEntityFrameworkCoreInstrumentation();
    traces.AddMongoDBInstrumentation();
});


await DefaultWebApplication.RunAsync(app);
