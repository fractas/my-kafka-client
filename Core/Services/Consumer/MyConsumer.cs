using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry.Serdes;

using Google.Protobuf;

using MyKafkaClient.Core.Mappings;
using MyKafkaClient.Core.Models.Message;

namespace MyKafkaClient.Core.Services.Consumer;

public interface IMyConsumer<out TKey, out TValue> : IDisposable
{
    void ConsumeAtMostOnce(string topic, Action<IMyMessage<TKey, TValue>> messageHandler);
    void ConsumeAtLeastOnce(string topic, Action<IMyMessage<TKey, TValue>> messageHandler);
    void Consume(string topic, Action<IMyMessage<TKey, TValue>> messageHandler);
}

public sealed class MyConsumer<TKey, TValue> : IMyConsumer<TKey, TValue>
    where TValue : class, IMessage<TValue>, new()
{
    private readonly IConsumer<TKey, TValue> _consumer;

    public MyConsumer(IMyConsumerConfiguration configuration) : this(configuration.Build())
    {
    }

    public MyConsumer(IEnumerable<KeyValuePair<string, string>> configuration) : this(configuration.ToArray())
    {
    }

    private MyConsumer(params KeyValuePair<string, string>[] configuration)
    {
        _consumer =
            new ConsumerBuilder<TKey, TValue>(configuration).SetValueDeserializer(
                new ProtobufDeserializer<TValue>().AsSyncOverAsync()).Build();
    }

    public void ConsumeAtMostOnce(string topic, Action<IMyMessage<TKey, TValue>> messageHandler)
    {
        ConsumeResult<TKey, TValue> reply = Consume(topic);

        if (reply.Message is null)
        {
            return;
        }

        _consumer.StoreOffset(reply);
        _consumer.Commit(reply);

        messageHandler(reply.Message.Map());
    }

    public void ConsumeAtLeastOnce(string topic, Action<IMyMessage<TKey, TValue>> messageHandler)
    {
        ConsumeResult<TKey, TValue> reply = Consume(topic);

        if (reply.Message is null)
        {
            return;
        }

        messageHandler(reply.Message.Map());

        _consumer.StoreOffset(reply);
        _consumer.Commit(reply);
    }

    public void Consume(string topic, Action<IMyMessage<TKey, TValue>> messageHandler)
    {
        ConsumeResult<TKey, TValue> reply = Consume(topic);

        if (reply.Message is null)
        {
            return;
        }

        messageHandler(reply.Message.Map());
    }

    public void Dispose()
    {
        _consumer.Close();
        _consumer.Dispose();
    }

    private ConsumeResult<TKey, TValue> Consume(string topic)
    {
        _consumer.Subscribe(topic);

        return _consumer.Consume();
    }
}