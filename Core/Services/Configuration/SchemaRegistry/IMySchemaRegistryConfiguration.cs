namespace MyKafkaClient.Core.Services.Configuration.SchemaRegistry ;

    public interface IMySchemaRegistryConfiguration
    {
        public enum MyAuthorizationSource
        {
            UserInfo,
            SaslInherit
        }

        IMySchemaRegistryConfiguration HavingUrl(params string[] url);
        IMySchemaRegistryConfiguration HavingAuthorizationMechanism(MyAuthorizationSource source);
        IMySchemaRegistryConfiguration HavingCredentials(string username, string password);

        IEnumerable<KeyValuePair<string, string>> Build();
    }