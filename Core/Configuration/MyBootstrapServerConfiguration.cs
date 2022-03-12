using System.ComponentModel;
using System.Reflection;

namespace MyKafkaClient.Core.Configuration;

public interface IMyBootstrapServerConfiguration
{
  enum AuthorizationMechanismMode
  {
    GSSAPI,
    PLAIN,
    [Description("SCRAM-SHA-256")] SCRAM_SHA_256,
    [Description("SCRAM-SHA-512")] SCRAM_SHA_512,
    OAUTHBEARER
  }

  string ClientId { get; set; }
  string Url { get; set; }
  string Username { get; set; }
  string Password { get; set; }
  AuthorizationMechanismMode AuthorizationMechanism { get; set; }

  IEnumerable<KeyValuePair<string, string>> GetConfiguration();
}

public class MyBootstrapServerConfiguration : IMyBootstrapServerConfiguration
{
  private readonly IDictionary<string, string> _configuration;
  private readonly string _id = Assembly.GetExecutingAssembly().GetName().Name!;

  public MyBootstrapServerConfiguration()
  {
    _configuration = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
      { "client.id", _id },
      { "bootstrap.servers", "localhost:9092" },
      { "sasl.username", "" },
      { "sasl.password", "" },
      { "sasl.mechanism", "PLAIN" }
    };
  }

  public string ClientId
  {
    get => _configuration["client.id"];
    set => _configuration["client.id"] = string.IsNullOrWhiteSpace(value) ? _id : value;
  }

  public string Url
  {
    get => _configuration["bootstrap.servers"];
    set
    {
      if (string.IsNullOrWhiteSpace(value))
      {
        throw new ArgumentNullException(nameof(Url));
      }

      _configuration["bootstrap.servers"] = value;
    }
  }

  public string Username
  {
    get => _configuration["sasl.username"];
    set => _configuration["sasl.username"] = value;
  }

  public string Password
  {
    get => _configuration["sasl.password"];
    set => _configuration["sasl.password"] = value;
  }

  public IMyBootstrapServerConfiguration.AuthorizationMechanismMode AuthorizationMechanism
  {
    get => Enum.Parse<IMyBootstrapServerConfiguration.AuthorizationMechanismMode>(_configuration["sasl.mechanism"]
      .Replace("-", "_"), true);
    set => _configuration["sasl.mechanism"] = value.GetType()
      .GetMember(value.ToString())
      .First()
      .GetCustomAttribute<DescriptionAttribute>()?
      .Description ?? Enum.GetName(value)!;
  }

  public IEnumerable<KeyValuePair<string, string>> GetConfiguration()
  {
    return _configuration.Where(w => !string.IsNullOrWhiteSpace(w.Value)).ToList();
  }
}
