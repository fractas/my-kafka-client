namespace MyKafkaClient.Core.Services.Configuration.BootstrapServer ;

    public interface IMyBootstrapServerConfiguration
    {
        enum MyAuthorizationMechanism
        {
            Gssapi,
            Plain,
            ScramSha256,
            ScramSha512,
            OAuthBearer
        }

        IMyBootstrapServerConfiguration HavingUrl(params string[] addresses);
        IMyBootstrapServerConfiguration HavingAuthorizationMechanism(MyAuthorizationMechanism mechanism);
        IMyBootstrapServerConfiguration HavingCredentials(string username, string password);

        IEnumerable<KeyValuePair<string, string>> Build();
    }