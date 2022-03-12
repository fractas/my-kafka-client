using Google.Protobuf;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using MyKafkaClient.Core.Interfaces;
using MyKafkaClient.Core.Services;

namespace KafkaClient.Core.Services;

public class MyConsumerBackgroundService<TMessage> : BackgroundService
  where TMessage : class, IMessage<TMessage>, new()
{
  private readonly IMyConsumerHandler<TMessage> _handler;
  private readonly ILogger<MyConsumerBackgroundService<TMessage>> _logger;
  private readonly IMyConsumerService<TMessage> _service;

  public MyConsumerBackgroundService(
    ILogger<MyConsumerBackgroundService<TMessage>> logger,
    IMyConsumerService<TMessage> service,
    IMyConsumerHandler<TMessage> handler)
  {
    _logger = logger;
    _service = service;
    _handler = handler;
  }

  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
#if RELEASE
    await Task.Yield();
#endif

    while (!stoppingToken.IsCancellationRequested)
    {
      try
      {
        _service.Consume(_handler.Handle, stoppingToken);
      }
      catch (OperationCanceledException exception)
      {
        throw RegisterFailure(exception);
      }
      catch (Exception exception)
      {
        RegisterFailure(exception);
      }

      await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
    }

    stoppingToken.ThrowIfCancellationRequested();
  }

  private Exception RegisterFailure(Exception exception)
  {
    _logger.LogError(exception, "{Message}", exception.InnerException?.Message ?? exception.Message);

    return exception;
  }
}
