namespace MyKafkaClient.Core.Configuration;

public interface IMySchemaRegistryConfiguration
{
  public enum AuthorizationSource
  {
    URL,
    USER_INFO,
    SASL_INHERIT
  }

  public string Url { get; set; }
  public AuthorizationSource CredentialSource { get; set; }
  public string Credential { get; set; }

  IEnumerable<KeyValuePair<string, string>> GetConfiguration();
}

public class MySchemaRegistryConfiguration : IMySchemaRegistryConfiguration
{
  private readonly IDictionary<string, string> _configuration;

  public MySchemaRegistryConfiguration()
  {
    _configuration = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
      { "schema.registry.url", "http://localhost:8081" },
      { "basic.auth.credentials.source", "" },
      { "basic.auth.user.info", "" }
    };
  }

  public string Url
  {
    get => _configuration["schema.registry.url"];
    set
    {
      if (string.IsNullOrWhiteSpace(value))
      {
        throw new ArgumentNullException(nameof(Url));
      }

      _configuration["schema.registry.url"] = value;
    }
  }

  public IMySchemaRegistryConfiguration.AuthorizationSource CredentialSource
  {
    get => Enum.TryParse(_configuration["basic.auth.credentials.source"],
      out IMySchemaRegistryConfiguration.AuthorizationSource value)
      ? value
      : IMySchemaRegistryConfiguration.AuthorizationSource.URL;
    set => _configuration["basic.auth.credentials.source"] =
      value == IMySchemaRegistryConfiguration.AuthorizationSource.URL ? "" : Enum.GetName(value)!;
  }

  public string Credential
  {
    get => _configuration["basic.auth.user.info"];
    set => _configuration["basic.auth.user.info"] = value;
  }

  public IEnumerable<KeyValuePair<string, string>> GetConfiguration()
  {
    return _configuration.Where(w => !string.IsNullOrWhiteSpace(w.Value)).ToList();
  }
}
