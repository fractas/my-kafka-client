using Confluent.Kafka;

using MyKafkaClient.Core.Services.Configuration.BootstrapServer;

namespace MyKafkaClient.Core.Services.Consumer;

public interface IMyConsumerConfiguration
{
    enum MyAck
    {
        All = -1,
        None = 0,
        Leader = 1
    }

    public enum MyOffsetReset
    {
        Latest,
        Earliest,
        Error
    }

    IMyConsumerConfiguration WithAck(MyAck ack);
    IMyConsumerConfiguration WithGroupId(string groupId);
    IMyConsumerConfiguration WithAutoOffsetReset(MyOffsetReset mode);
    IMyConsumerConfiguration WithAutoCommit(bool? enable = true);
    IMyConsumerConfiguration WithAutoOffsetStore(bool? enable = false);
    IMyConsumerConfiguration WithAutoCreateTopics(bool? enable = false);
    IMyConsumerConfiguration WithPartitionEof(bool? enable = false);
    IMyConsumerConfiguration WithClientId(string clientId);
    IMyConsumerConfiguration WithAutoCommitInterval(TimeSpan intervalInMilliseconds);
    IMyConsumerConfiguration WithMaxPollInterval(TimeSpan intervalInMilliseconds);
    IMyConsumerConfiguration WithSessionTimeout(TimeSpan intervalInMilliseconds);
    IMyConsumerConfiguration WithMetadataMaxAge(TimeSpan intervalInMilliseconds);
    IMyConsumerConfiguration WithBootstrapServer(Action<IMyBootstrapServerConfiguration> builder);

    IEnumerable<KeyValuePair<string, string>> Build();
}

public class MyConsumerConfiguration : IMyConsumerConfiguration
{
    private readonly IDictionary<string, string> _items;

    public MyConsumerConfiguration()
    {
        _ = new ConsumerConfig();

        _items = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
    }

    public IMyConsumerConfiguration WithAck(IMyConsumerConfiguration.MyAck ack)
    {
        int selected = ack switch
        {
            IMyConsumerConfiguration.MyAck.None => 0,
            IMyConsumerConfiguration.MyAck.Leader => 1,
            IMyConsumerConfiguration.MyAck.All => -1,
            _ => throw new ArgumentOutOfRangeException(nameof(ack), ack, null)
        };

        Merge("acks", $"{selected}");
        return this;
    }

    public IMyConsumerConfiguration WithGroupId(string groupId)
    {
        ArgumentNullException.ThrowIfNull(groupId);

        Merge("group.id", groupId);
        return this;
    }

    // { "default": "largest" }
    public IMyConsumerConfiguration WithAutoOffsetReset(IMyConsumerConfiguration.MyOffsetReset mode)
    {
        Merge("auto.offset.reset", mode.ToString().ToLower());
        return this;
    }

    // { "default": true }
    public IMyConsumerConfiguration WithAutoCommit(bool? enable = true)
    {
        ArgumentNullException.ThrowIfNull(enable);

        Merge("enable.auto.commit", enable.GetValueOrDefault().ToString().ToLower());
        return this;
    }

    // { "default": true }
    public IMyConsumerConfiguration WithAutoOffsetStore(bool? enable = false)
    {
        ArgumentNullException.ThrowIfNull(enable);

        Merge("enable.auto.offset.store", enable.GetValueOrDefault().ToString().ToLower());
        return this;
    }

    // { "default": false }
    public IMyConsumerConfiguration WithAutoCreateTopics(bool? enable = false)
    {
        ArgumentNullException.ThrowIfNull(enable);

        Merge("allow.auto.create.topics", enable.GetValueOrDefault().ToString().ToLower());
        return this;
    }

    // { "default": false }
    public IMyConsumerConfiguration WithPartitionEof(bool? enable = false)
    {
        ArgumentNullException.ThrowIfNull(enable);

        Merge("enable.partition.eof", enable.GetValueOrDefault().ToString().ToLower());
        return this;
    }

    public IMyConsumerConfiguration WithClientId(string clientId)
    {
        ArgumentNullException.ThrowIfNull(clientId);

        Merge("client.id", clientId);
        return this;
    }

    // { "default": 5000 }
    public IMyConsumerConfiguration WithAutoCommitInterval(TimeSpan intervalInMilliseconds)
    {
        ArgumentNullException.ThrowIfNull(intervalInMilliseconds);

        Merge("auto.commit.interval.ms", $"{intervalInMilliseconds.TotalMilliseconds}");
        return this;
    }

    // { "default": 300000 }
    public IMyConsumerConfiguration WithMaxPollInterval(TimeSpan intervalInMilliseconds)
    {
        ArgumentNullException.ThrowIfNull(intervalInMilliseconds);

        Merge("max.poll.interval.ms", $"{intervalInMilliseconds.TotalMilliseconds}");
        return this;
    }

    // { "default": 45000 }
    public IMyConsumerConfiguration WithSessionTimeout(TimeSpan intervalInMilliseconds)
    {
        ArgumentNullException.ThrowIfNull(intervalInMilliseconds);

        Merge("session.timeout.ms", $"{intervalInMilliseconds.TotalMilliseconds}");
        return this;
    }

    // { "default": 900000 }
    public IMyConsumerConfiguration WithMetadataMaxAge(TimeSpan intervalInMilliseconds)
    {
        ArgumentNullException.ThrowIfNull(intervalInMilliseconds);

        Merge("metadata.max.age.ms", $"{intervalInMilliseconds.TotalMilliseconds}");
        return this;
    }

    public IMyConsumerConfiguration WithBootstrapServer(Action<IMyBootstrapServerConfiguration> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        MyBootstrapServerConfiguration configuration = new();
        builder(configuration);
        configuration.Build().ToList().ForEach(each => Merge(each.Key, each.Value));
        return this;
    }

    public IEnumerable<KeyValuePair<string, string>> Build()
    {
        if (_items.TryGetValue("group.id", out string? groupId))
        {
            ArgumentNullException.ThrowIfNull(groupId);
        }

        return _items;
    }

    private void Merge(string key, string value)
    {
        if (!_items.TryAdd(key, value))
        {
            _items[key] = value;
        }
    }
}