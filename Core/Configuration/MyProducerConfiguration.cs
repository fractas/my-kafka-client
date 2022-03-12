namespace MyKafkaClient.Core.Configuration;

public interface IMyProducerConfiguration
{
  enum MyAcknowledgements
  {
    All = -1,
    None = 0,
    Leader = 1
  }

  string Topic { get; set; }
  MyAcknowledgements Acknowledgement { get; set; }
  bool EnableIdempotence { get; set; }
  bool EnableDeliveryReports { get; set; }
  int RetryBackoff { get; set; }
  int MessageSendMaxRetries { get; set; }

  void SetBootstrapServer(IMyBootstrapServerConfiguration bootstrapServer);
  void SetSchemaRegistry(IMySchemaRegistryConfiguration schemaRegistry);
  IEnumerable<KeyValuePair<string, string>> GetSchemaRegistry();

  IEnumerable<KeyValuePair<string, string>> GetConfiguration();
}

public class MyProducerConfiguration : IMyProducerConfiguration
{
  private readonly IDictionary<string, string> _configuration;

  private IMySchemaRegistryConfiguration _schemaRegistry = new MySchemaRegistryConfiguration();

  private string _topic = string.Empty;

  public MyProducerConfiguration()
  {
    _configuration = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
      { "acks", "-1" },
      { "enable.idempotence", "false" },
      { "dotnet.producer.enable.delivery.reports", "true" },
      { "retry.backoff.ms", "1000" },
      { "message.send.max.retries", "3" }
    };
  }

  public string Topic
  {
    get => _topic;
    set
    {
      if (string.IsNullOrWhiteSpace(value))
      {
        throw new ArgumentNullException(nameof(Topic));
      }

      _topic = value;
    }
  }

  public IMyProducerConfiguration.MyAcknowledgements Acknowledgement
  {
    get => Enum.Parse<IMyProducerConfiguration.MyAcknowledgements>(int.Parse(_configuration["acks"]).ToString());
    set => _configuration["acks"] = ((int)value).ToString();
  }

  public bool EnableIdempotence
  {
    get => bool.Parse(_configuration["enable.idempotence"]);
    set => _configuration["enable.idempotence"] = value.ToString().ToLower();
  }

  public bool EnableDeliveryReports
  {
    get => bool.Parse(_configuration["dotnet.producer.enable.delivery.reports"]);
    set => _configuration["dotnet.producer.enable.delivery.reports"] = value.ToString().ToLower();
  }

  public int RetryBackoff
  {
    get => int.Parse(_configuration["retry.backoff.ms"]);
    set => _configuration["retry.backoff.ms"] = value <= 0 ? 1000.ToString() : value.ToString();
  }

  public int MessageSendMaxRetries
  {
    get => int.Parse(_configuration["message.send.max.retries"]);
    set => _configuration["message.send.max.retries"] = value <= 0 ? 3.ToString() : value.ToString();
  }

  public void SetBootstrapServer(IMyBootstrapServerConfiguration bootstrapServer)
  {
    ArgumentNullException.ThrowIfNull(bootstrapServer);

    foreach ((string key, string value) in bootstrapServer.GetConfiguration())
    {
      _configuration.Add(key, value);
    }
  }

  public void SetSchemaRegistry(IMySchemaRegistryConfiguration schemaRegistry)
  {
    ArgumentNullException.ThrowIfNull(schemaRegistry);

    _schemaRegistry = schemaRegistry;
  }

  public IEnumerable<KeyValuePair<string, string>> GetSchemaRegistry()
  {
    return _schemaRegistry.GetConfiguration();
  }

  public IEnumerable<KeyValuePair<string, string>> GetConfiguration()
  {
    if (string.IsNullOrWhiteSpace(Topic))
    {
      throw new ArgumentNullException(nameof(Topic));
    }

    return _configuration.Where(w => !string.IsNullOrWhiteSpace(w.Value)).ToList();
  }
}
