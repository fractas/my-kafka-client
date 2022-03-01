using MyKafkaClient.Core.Services.Configuration.BootstrapServer;
using MyKafkaClient.Core.Services.Configuration.SchemaRegistry;

namespace MyKafkaClient.Core.Services.Producer;

public interface IMyProducerConfiguration
{
    enum MyAck
    {
        All = -1,
        None = 0,
        Leader = 1
    }

    IMyProducerConfiguration WithAck(MyAck ack);
    IMyProducerConfiguration WithClientId(string clientId);
    IMyProducerConfiguration WithMessageTimeout(TimeSpan timeoutInMilliseconds);

    IMyProducerConfiguration WithBootstrapServer(Action<IMyBootstrapServerConfiguration> builder);
    IMyProducerConfiguration WithSchemaRegistry(Action<IMySchemaRegistryConfiguration> builder);


    IEnumerable<KeyValuePair<string, string>> Build();
}

public class MyProducerConfiguration : IMyProducerConfiguration
{
    private readonly IDictionary<string, string> _items;

    public MyProducerConfiguration()
    {
        _items = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
    }

    public IMyProducerConfiguration WithAck(IMyProducerConfiguration.MyAck ack)
    {
        int selected = ack switch
        {
            IMyProducerConfiguration.MyAck.None => 0,
            IMyProducerConfiguration.MyAck.Leader => 1,
            IMyProducerConfiguration.MyAck.All => -1,
            _ => throw new ArgumentOutOfRangeException(nameof(ack), ack, null)
        };

        Merge("acks", $"{selected}");
        return this;
    }

    public IMyProducerConfiguration WithClientId(string clientId)
    {
        ArgumentNullException.ThrowIfNull(clientId);

        Merge("client.id", clientId);
        return this;
    }

    public IMyProducerConfiguration WithMessageTimeout(TimeSpan timeoutInMilliseconds)
    {
        ArgumentNullException.ThrowIfNull(timeoutInMilliseconds);

        Merge("message.timeout.ms", $"{timeoutInMilliseconds.Milliseconds}");
        return this;
    }

    public IMyProducerConfiguration WithBootstrapServer(Action<IMyBootstrapServerConfiguration> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        MyBootstrapServerConfiguration configuration = new();
        builder(configuration);
        configuration.Build().ToList().ForEach(each => Merge(each.Key, each.Value));
        return this;
    }

    public IMyProducerConfiguration WithSchemaRegistry(Action<IMySchemaRegistryConfiguration> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        MySchemaRegistryConfiguration configuration = new();
        builder(configuration);
        configuration.Build().ToList().ForEach(each => Merge(each.Key, each.Value));
        return this;
    }

    public IEnumerable<KeyValuePair<string, string>> Build()
    {
        return _items;
    }

    public IMyProducerConfiguration WithMaxPollInterval(TimeSpan intervalInMilliseconds)
    {
        ArgumentNullException.ThrowIfNull(intervalInMilliseconds);

        Merge("max.poll.interval.ms", $"{intervalInMilliseconds.Milliseconds}");
        return this;
    }

    public IMyProducerConfiguration WithMetadataMaxAge(TimeSpan intervalInMilliseconds)
    {
        ArgumentNullException.ThrowIfNull(intervalInMilliseconds);

        Merge("metadata.max.age.ms", $"{intervalInMilliseconds.Milliseconds}");
        return this;
    }

    private void Merge(string key, string value)
    {
        if (!_items.TryAdd(key, value))
        {
            _items[key] = value;
        }
    }
}