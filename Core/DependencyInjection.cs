using Google.Protobuf;

using KafkaClient.Core.Services;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

using MyKafkaClient.Core.Configuration;
using MyKafkaClient.Core.Interfaces;
using MyKafkaClient.Core.Services;

namespace KafkaClient.Core;

public static class DependencyInjection
{
  public static IServiceCollection AddConsumer<TMessage, TMessageHandler>(this IServiceCollection services,
    string topic, MyClientConfiguration settings)
    where TMessage : class, IMessage<TMessage>, new()
    where TMessageHandler : IMyConsumerHandler<TMessage>
  {
    ArgumentNullException.ThrowIfNull(settings);

    MyConsumerConfiguration? consumer = settings.Consumers.FirstOrDefault(first => first.Topic.Equals(topic));

    ArgumentNullException.ThrowIfNull(consumer);
    ArgumentNullException.ThrowIfNull(settings.BootstrapServer);

    consumer.SetBootstrapServer(settings.BootstrapServer);

    services.TryAdd(ServiceDescriptor.Singleton(typeof(IMyConsumerHandler<TMessage>), typeof(TMessageHandler)));
    services.TryAddSingleton<IMyConsumerService<TMessage>>(s =>
      new MyConsumerService<TMessage>(s.GetRequiredService<ILogger<MyConsumerService<TMessage>>>(), consumer));

    services.AddHostedService<MyConsumerBackgroundService<TMessage>>();

    return services;
  }

  public static IServiceCollection AddProducer<TMessage>(this IServiceCollection services,
    string topic, MyClientConfiguration settings)
    where TMessage : class, IMessage<TMessage>, new()
  {
    ArgumentNullException.ThrowIfNull(settings);

    MyProducerConfiguration? producer = settings.Producers.FirstOrDefault(first => first.Topic.Equals(topic));

    ArgumentNullException.ThrowIfNull(producer);
    ArgumentNullException.ThrowIfNull(settings.BootstrapServer);
    ArgumentNullException.ThrowIfNull(settings.SchemaRegistry);

    producer.SetBootstrapServer(settings.BootstrapServer);
    producer.SetSchemaRegistry(settings.SchemaRegistry);

    services.TryAddSingleton<IMyProducerService<TMessage>>(s =>
      new MyProducerService<TMessage>(s.GetRequiredService<ILogger<MyProducerService<TMessage>>>(), producer));

    return services;
  }
}
