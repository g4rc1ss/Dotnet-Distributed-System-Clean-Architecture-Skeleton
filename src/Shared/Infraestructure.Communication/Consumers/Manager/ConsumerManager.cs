namespace Infraestructure.Communication.Consumers.Manager;

public class ConsumerManager<TMessage> : IConsumerManager<TMessage>
{
    private CancellationTokenSource _cancellationTokenSource;

    public ConsumerManager()
    {
        _cancellationTokenSource = new CancellationTokenSource();
    }

    public CancellationToken GetCancellationToken()
    {
        return _cancellationTokenSource.Token;
    }

    public void RestartExecution()
    {
        var cancellationTokenSource = _cancellationTokenSource;
        _cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Cancel();
    }

    public void StopExecution()
    {
        _cancellationTokenSource.Cancel();
    }
}

