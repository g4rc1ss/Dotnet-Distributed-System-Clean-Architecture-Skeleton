﻿using System.Diagnostics;
using Infraestructure.Communication.Consumers.Manager;
using Microsoft.Extensions.Hosting;

namespace Infraestructure.Communication.Consumers.Host;

public class ConsumerHostedService<TMessage> : IHostedService
{
    private readonly IConsumerManager<TMessage> _consumerManager;
    private readonly IMessageConsumer<TMessage> _messageConsumer;
    private readonly CancellationTokenSource _stoppingCancellationTokenSource = new CancellationTokenSource();
    private Task? _executingTask;

    public ConsumerHostedService(IConsumerManager<TMessage> consumerManager, IMessageConsumer<TMessage> messageConsumer)
    {
        _consumerManager = consumerManager;
        _messageConsumer = messageConsumer;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _executingTask = ConsumeMessages(_stoppingCancellationTokenSource.Token);
        return _executingTask.IsCompleted ? _executingTask : Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _stoppingCancellationTokenSource.Cancel();
        _consumerManager.StopExecution();
        return Task.CompletedTask;
    }

    private async Task ConsumeMessages(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var ct = _consumerManager.GetCancellationToken();
            if (ct.IsCancellationRequested)
            {
                break;
            }

            try
            {
                await _messageConsumer.StartAsync(cancellationToken);
                await Task.Delay(1000, cancellationToken);
            }
            catch (OperationCanceledException)
            {
            }
        }
    }
}

