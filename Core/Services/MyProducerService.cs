using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;

using Google.Protobuf;

using Microsoft.Extensions.Logging;

using MyKafkaClient.Core.Configuration;
using MyKafkaClient.Core.Models;

namespace MyKafkaClient.Core.Services;

public interface IMyProducerService<TMessage> : IDisposable
{
  void Produce(string topic, MyMessage<TMessage> message);
}

public sealed class MyProducerService<TMessage> : IMyProducerService<TMessage>
  where TMessage : class, IMessage<TMessage>, new()
{
  private readonly ILogger<MyProducerService<TMessage>> _logger;
  private readonly IProducer<string, TMessage> _producer;

  public MyProducerService(ILogger<MyProducerService<TMessage>> logger, MyProducerConfiguration configuration)
  {
    ArgumentNullException.ThrowIfNull(configuration);

    _logger = logger;
    _producer = new ProducerBuilder<string, TMessage>(configuration.GetConfiguration())
      .SetValueSerializer(GetSchemaRegistrySerializer(configuration.GetSchemaRegistry()))
      .SetErrorHandler((_, e) =>
        _logger.LogError("Producer Error: {Reason}. Is Fatal: {IsFatal}", e.Reason, e.IsFatal))
      .SetLogHandler((_, l) =>
        _logger.LogInformation("Producer Info: {Facility}-{Level} Message: {Message}", l.Facility, l.Level, l.Message))
      .Build();
  }

  public void Produce(string topic, MyMessage<TMessage> message)
  {
    try
    {
      _producer.Produce(topic, message.Map(), Handle);
    }
    catch (Exception exception)
    {
      _logger.LogError(exception, "Producing Error: {Message} for message (value: '{Value}')",
        exception.InnerException?.Message ?? exception.Message, message.Value);

      throw;
    }
  }

  public void Dispose()
  {
    _producer.Dispose();
  }

  private static ISerializer<TMessage> GetSchemaRegistrySerializer(
    IEnumerable<KeyValuePair<string, string>> options)
  {
    return new ProtobufSerializer<TMessage>(GetSchemaRegistryClient(options)).AsSyncOverAsync();
  }

  private static ISchemaRegistryClient GetSchemaRegistryClient(IEnumerable<KeyValuePair<string, string>> options)
  {
    return new CachedSchemaRegistryClient(options);
  }

  private void Handle(DeliveryResult<string, TMessage> handler)
  {
    if (handler.Status != PersistenceStatus.Persisted)
    {
      _logger.LogError("Delivery Error: [{Status}] Message not acknowledged by all brokers (value: '{Message}')",
        handler.Status, handler.Message);
    }

    Thread.Sleep(TimeSpan.FromSeconds(2));
  }
}
