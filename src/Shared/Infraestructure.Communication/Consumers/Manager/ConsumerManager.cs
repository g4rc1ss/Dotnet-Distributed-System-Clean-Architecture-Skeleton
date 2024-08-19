namespace Infraestructure.Communication.Consumers.Manager;

public class ConsumerManager<TMessage> : IConsumerManager<TMessage>
{
    private CancellationTokenSource cancellationTokenSource;

    public ConsumerManager()
    {
        cancellationTokenSource = new CancellationTokenSource();
    }

    public CancellationToken GetCancellationToken()
    {
        return cancellationTokenSource.Token;
    }

    public void RestartExecution()
    {
        var cancellationTokenSource = this.cancellationTokenSource;
        this.cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Cancel();
    }

    public void StopExecution()
    {
        cancellationTokenSource.Cancel();
    }
}

