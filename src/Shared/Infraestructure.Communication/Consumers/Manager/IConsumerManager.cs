using System;
namespace Infraestructure.Communication.Consumers.Manager
{
    public interface IConsumerManager<TMessage>
    {
        void RestartExecution();
        void StopExecution();
        CancellationToken GetCancellationToken();
    }
}

