using System;
using Infraestructure.Communication.Consumers.Host;
using Infraestructure.Communication.Consumers.Manager;
using Infraestructure.Communication.Messages;
using Microsoft.AspNetCore.Mvc;

namespace WeatherForecast.Consumer.SyncCreateData.Controllers;

[ApiController]
[Route("[controller]")]
public class SyncWeatherDbController : ConsumerController<IntegrationMessage>
{
    public SyncWeatherDbController(IConsumerManager<IntegrationMessage> consumerManager)
        : base(consumerManager)
    {
    }
}

