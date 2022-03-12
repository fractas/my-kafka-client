namespace MyKafkaClient.Core.Configuration;

public interface IMyConsumerConfiguration
{
  public enum MyOffsetReset
  {
    Latest,
    Earliest,
    Error
  }

  string Topic { get; set; }
  string GroupId { get; set; }
  MyOffsetReset AutoOffsetReset { get; set; }
  bool AutoCommit { get; set; }
  bool AutoOffsetStore { get; set; }
  int MaxPollInterval { get; set; }

  void SetBootstrapServer(IMyBootstrapServerConfiguration bootstrapServer);

  IEnumerable<KeyValuePair<string, string>> GetConfiguration();
}

public class MyConsumerConfiguration : IMyConsumerConfiguration
{
  private readonly IDictionary<string, string> _configuration;

  private string _topic = string.Empty;

  public MyConsumerConfiguration()
  {
    _configuration = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
      { "group.id", "" },
      { "auto.offset.reset", "earliest" },
      { "enable.auto.commit", "false" },
      { "enable.auto.offset.store", "false" },
      { "max.poll.interval.ms", "250000" }
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

  public string GroupId
  {
    get => _configuration["group.id"]!;
    set
    {
      if (string.IsNullOrWhiteSpace(value))
      {
        throw new ArgumentNullException(nameof(GroupId));
      }

      _configuration["group.id"] = value;
    }
  }

  public IMyConsumerConfiguration.MyOffsetReset AutoOffsetReset
  {
    get => Enum.Parse<IMyConsumerConfiguration.MyOffsetReset>(_configuration["auto.offset.reset"], true);
    set => _configuration["auto.offset.reset"] = Enum.GetName(value)!.ToLower();
  }

  public bool AutoCommit
  {
    get => bool.Parse(_configuration["enable.auto.commit"]);
    set => _configuration["enable.auto.commit"] = value.ToString().ToLower();
  }

  public bool AutoOffsetStore
  {
    get => bool.Parse(_configuration["enable.auto.offset.store"]);
    set => _configuration["enable.auto.offset.store"] = value.ToString().ToLower();
  }

  public int MaxPollInterval
  {
    get => int.Parse(_configuration["max.poll.interval.ms"]);
    set => _configuration["max.poll.interval.ms"] = value <= 0 ? 250000.ToString() : value.ToString();
  }

  public void SetBootstrapServer(IMyBootstrapServerConfiguration bootstrapServer)
  {
    ArgumentNullException.ThrowIfNull(bootstrapServer);

    foreach ((string key, string value) in bootstrapServer.GetConfiguration())
    {
      if (_configuration.TryAdd(key, value))
      {
        _configuration[key] = value;
      }
    }
  }

  public IEnumerable<KeyValuePair<string, string>> GetConfiguration()
  {
    if (string.IsNullOrWhiteSpace(Topic))
    {
      throw new ArgumentNullException(nameof(Topic));
    }

    if (string.IsNullOrWhiteSpace(GroupId))
    {
      throw new ArgumentNullException(nameof(GroupId));
    }

    return _configuration.Where(w => !string.IsNullOrWhiteSpace(w.Value)).ToList();
  }
}
