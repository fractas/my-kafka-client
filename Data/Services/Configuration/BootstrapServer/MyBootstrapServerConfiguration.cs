using MyKafkaClient.Core.Services.Configuration.BootstrapServer;

namespace MyKafkaClient.Data.Services.Configuration.BootstrapServer ;

    public class MyBootstrapServerConfiguration : IMyBootstrapServerConfiguration
    {
        private readonly IDictionary<string, string> _items;

        public MyBootstrapServerConfiguration()
        {
            _items = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        public IMyBootstrapServerConfiguration HavingUrl(params string[] addresses)
        {
            ArgumentNullException.ThrowIfNull(addresses);

            Merge("bootstrap.servers", string.Join(',', addresses));
            return this;
        }

        public IMyBootstrapServerConfiguration HavingAuthorizationMechanism(
            IMyBootstrapServerConfiguration.MyAuthorizationMechanism mechanism)
        {
            var selected = mechanism switch 
            {
                IMyBootstrapServerConfiguration.MyAuthorizationMechanism.Gssapi => "GSSAPI",
                IMyBootstrapServerConfiguration.MyAuthorizationMechanism.Plain => "PLAIN",
                IMyBootstrapServerConfiguration.MyAuthorizationMechanism.ScramSha256 => "SCRAM-SHA-256",
                IMyBootstrapServerConfiguration.MyAuthorizationMechanism.ScramSha512 => "SCRAM-SHA-512",
                IMyBootstrapServerConfiguration.MyAuthorizationMechanism.OAuthBearer => "OAUTHBEARER",
                _ => throw new ArgumentOutOfRangeException(nameof(mechanism), mechanism, null)
                };

            Merge("sasl.mechanism", selected);
            return this;
        }

        public IMyBootstrapServerConfiguration HavingCredentials(string username, string password)
        {
            ArgumentNullException.ThrowIfNull(username);
            ArgumentNullException.ThrowIfNull(password);

            Merge("sasl.username", username);
            Merge("sasl.password", password);
            return this;
        }

        public IEnumerable<KeyValuePair<string, string>> Build()
        {
            return _items;
        }

        private void Merge(string key, string value)
        {
            if (!_items.TryAdd(key, value))
                _items[key] = value;
        }
    }