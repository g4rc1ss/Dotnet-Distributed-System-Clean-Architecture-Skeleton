using System;
using Infraestructure.Communication.Consumers.Manager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Infraestructure.Communication.Consumers.Host
{
    public class ConsumerController<TMessage> : Controller
    {
        public readonly IConsumerManager<TMessage> _consumerManager;

        public ConsumerController(IConsumerManager<TMessage> consumerManager)
        {
            _consumerManager = consumerManager;
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("start")]
        public virtual IActionResult Start()
        {
            _consumerManager.RestartExecution();
            return Ok();
        }
    }
}

