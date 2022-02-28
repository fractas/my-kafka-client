using MyKafkaClient.Core.Services.Configuration.SchemaRegistry;

namespace MyKafkaClient.Data.Services.Configuration.SchemaRegistry ;

    public class MySchemaRegistryConfiguration : IMySchemaRegistryConfiguration
    {
        private readonly IDictionary<string, string> _items;

        public MySchemaRegistryConfiguration()
        {
            _items = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        public IMySchemaRegistryConfiguration HavingUrl(params string[] url)
        {
            ArgumentNullException.ThrowIfNull(url);

            Merge("schema.registry.url", string.Join(',', url));
            return this;
        }

        public IMySchemaRegistryConfiguration HavingAuthorizationMechanism(
            IMySchemaRegistryConfiguration.MyAuthorizationSource source)
        {
            var selected = source switch 
            {
                IMySchemaRegistryConfiguration.MyAuthorizationSource.SaslInherit => "SASL_INHERIT",
                IMySchemaRegistryConfiguration.MyAuthorizationSource.UserInfo => "USER_INFO",
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null)
                };

            Merge("schema.registry.basic.auth.credentials.source", selected);
            return this;
        }

        public IMySchemaRegistryConfiguration HavingCredentials(string username, string password)
        {
            ArgumentNullException.ThrowIfNull(username);
            ArgumentNullException.ThrowIfNull(password);

            Merge("schema.registry.basic.auth.user.info", $"{username}:{password}");
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