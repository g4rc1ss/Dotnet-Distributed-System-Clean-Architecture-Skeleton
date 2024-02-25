using Infraestructure.Communication.Consumers.Manager;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Infraestructure.Communication.Consumers.Host;

public class ConsumerController<TMessage>(IConsumerManager<TMessage> consumerManager) : Controller
{
    public readonly IConsumerManager<TMessage> consumerManager = consumerManager;

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Route("start")]
    public virtual IActionResult Start()
    {
        consumerManager.RestartExecution();
        return Ok();
    }
}

