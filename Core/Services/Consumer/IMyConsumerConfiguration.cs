using MyKafkaClient.Core.Services.Configuration.BootstrapServer;

namespace MyKafkaClient.Core.Services.Consumer ;

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