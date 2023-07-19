
namespace Infraestructure.Communication.Consumers;

public interface IMessageConsumer
{
    Task StartAsync(CancellationToken cancellationToken = default);
}

public interface IMessageConsumer<T> : IMessageConsumer
{

}
