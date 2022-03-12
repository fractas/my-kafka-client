using MyKafkaClient.Core.Configuration;

namespace KafkaClient.Core;

public class MyClientConfiguration
{
  public MyBootstrapServerConfiguration BootstrapServer { get; set; } = new();
  public MySchemaRegistryConfiguration SchemaRegistry { get; set; } = new();
  public List<MyConsumerConfiguration> Consumers { get; set; } = new();
  public List<MyProducerConfiguration> Producers { get; set; } = new();
}
