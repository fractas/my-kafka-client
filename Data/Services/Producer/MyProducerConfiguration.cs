using MyKafkaClient.Core.Services.Configuration.BootstrapServer;
using MyKafkaClient.Core.Services.Configuration.SchemaRegistry;
using MyKafkaClient.Core.Services.Producer;
using MyKafkaClient.Data.Services.Configuration.BootstrapServer;
using MyKafkaClient.Data.Services.Configuration.SchemaRegistry;

namespace MyKafkaClient.Data.Services.Producer ;


    public class MyProducerConfiguration : IMyProducerConfiguration
    {
        private readonly IDictionary<string, string> _items;

        public MyProducerConfiguration()
        {
            _items = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        public IMyProducerConfiguration WithAck(IMyProducerConfiguration.MyAck ack)
        {
            var selected = ack switch 
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

            var configuration = new MyBootstrapServerConfiguration();
            builder(configuration);
            configuration.Build().ToList().ForEach(each => Merge(each.Key, each.Value));
            return this;
        }

        public IMyProducerConfiguration WithSchemaRegistry(Action<IMySchemaRegistryConfiguration> builder)
        {
            ArgumentNullException.ThrowIfNull(builder);

            var configuration = new MySchemaRegistryConfiguration();
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
                _items[key] = value;
        }
    }