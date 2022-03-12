using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry.Serdes;

using Google.Protobuf;

using Microsoft.Extensions.Logging;

using MyKafkaClient.Core.Configuration;
using MyKafkaClient.Core.Models;

namespace MyKafkaClient.Core.Services;

public interface IMyConsumerService<TMessage> : IDisposable
{
  void Consume(Action<MyMessage<TMessage>> action, CancellationToken cancellationToken);
}

public sealed class MyConsumerService<TMessage> : IMyConsumerService<TMessage>
  where TMessage : class, IMessage<TMessage>, new()
{
  private readonly MyConsumerConfiguration _configuration;
  private readonly IConsumer<string, TMessage> _consumer;
  private readonly ILogger<MyConsumerService<TMessage>> _logger;

  public MyConsumerService(ILogger<MyConsumerService<TMessage>> logger, MyConsumerConfiguration configuration)
  {
    _logger = logger;
    _configuration = configuration;
    _consumer = new ConsumerBuilder<string, TMessage>(configuration.GetConfiguration())
      .SetValueDeserializer(new ProtobufDeserializer<TMessage>().AsSyncOverAsync())
      .SetErrorHandler((_, e) => _logger.LogError("Consumer Error: {Reason}", e.Reason))
      .Build();
    _consumer.Subscribe(configuration.Topic);
  }

  public void Consume(Action<MyMessage<TMessage>> action, CancellationToken cancellationToken)
  {
    try
    {
      ConsumeResult<string, TMessage>? result = _consumer.Consume(_configuration.MaxPollInterval);

      if (result?.Message?.Value is null || cancellationToken.IsCancellationRequested)
      {
        return;
      }

      action(result.Message.Map());

      _consumer.StoreOffset(result);
      _consumer.Commit(result);
    }
    catch (Exception exception)
    {
      _logger.LogError(exception, "Consuming Error: {Message}", exception.InnerException?.Message ?? exception.Message);
    }
    finally
    {
      cancellationToken.ThrowIfCancellationRequested();
    }
  }

  public void Dispose()
  {
    _consumer.Close();
  }
}

