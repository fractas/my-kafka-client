using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;

using Google.Protobuf;

using MyKafkaClient.Core.Mappings;
using MyKafkaClient.Core.Models.Message;

namespace MyKafkaClient.Core.Services.Producer;

public interface IMyProducer<in TKey, in TValue> : IDisposable
{
    void ProduceAndConfirm(string topic, IMyMessage<TKey, TValue> message);
    void Produce(string topic, IMyMessage<TKey, TValue> message);
}

public sealed class MyProducer<TKey, TValue> : IMyProducer<TKey, TValue>
    where TValue : class, IMessage<TValue>, new()
{
    private readonly IProducer<TKey, TValue> _producer;

    public MyProducer(IMyProducerConfiguration configuration) : this(configuration.Build())
    {
    }

    public MyProducer(IEnumerable<KeyValuePair<string, string>> configuration) : this(configuration.ToArray())
    {
    }

    private MyProducer(params KeyValuePair<string, string>[] configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        _producer =
            new ProducerBuilder<TKey, TValue>(GetProducerConfig(configuration)).SetValueSerializer(
                GetSchemaRegistrySerializer(configuration)).Build();
    }

    public void ProduceAndConfirm(string topic, IMyMessage<TKey, TValue> message)
    {
        Produce(topic, message, PersistenceStatus.Persisted.ToString());
    }

    public void Produce(string topic, IMyMessage<TKey, TValue> message)
    {
        Produce(topic, message, PersistenceStatus.PossiblyPersisted.ToString(),
            PersistenceStatus.Persisted.ToString());
    }

    public void Dispose()
    {
        _producer.Dispose();
    }

    private static IEnumerable<KeyValuePair<string, string>> GetProducerConfig(
        params KeyValuePair<string, string>[] configuration)
    {
        return configuration.Where(w => !w.Key.StartsWith("schema.registry"));
    }

    private static IAsyncSerializer<TValue> GetSchemaRegistrySerializer(
        params KeyValuePair<string, string>[] configuration)
    {
        return new ProtobufSerializer<TValue>(GetSchemaRegistryClient(configuration));
    }

    private static ISchemaRegistryClient GetSchemaRegistryClient(params KeyValuePair<string, string>[] configuration)
    {
        return new CachedSchemaRegistryClient(GetSchemaRegistryConfig(configuration));
    }

    private static IEnumerable<KeyValuePair<string, string>> GetSchemaRegistryConfig(
        params KeyValuePair<string, string>[] configuration)
    {
        return configuration.Where(w => w.Key.StartsWith("schema.registry") || w.Key.StartsWith("sasl"));
    }

    private void Produce(string topic, IMyMessage<TKey, TValue> message, params string[] state)
    {
        _producer.Produce(topic, message.Map(), report => Handle(report, state));
    }

    private static void Handle(DeliveryResult<TKey, TValue> handler, params string[] state)
    {
        if (state.Any(a => handler.Status == Enum.Parse<PersistenceStatus>(a)))
        {
            return;
        }

        throw new KafkaException(ErrorCode.Local_State);
    }
}