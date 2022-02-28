using MyKafkaClient.Core.Services.Configuration.BootstrapServer;
using MyKafkaClient.Core.Services.Configuration.SchemaRegistry;

namespace MyKafkaClient.Core.Services.Producer ;

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