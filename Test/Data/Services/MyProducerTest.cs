using System.Net;

using FluentAssertions;

using Moq;

using MyKafkaClient.Core.Services.Configuration.BootstrapServer;
using MyKafkaClient.Core.Services.Configuration.SchemaRegistry;
using MyKafkaClient.Core.Services.Producer;
using MyKafkaClient.Data.Services.Producer;

using NUnit.Framework;

namespace MyKafkaClient.Test.Data.Services ;

    public class MyProducerTest
    {
        [Test]
        public void MyProducer_SchemaRegistry_ShouldBeRequired()
        {
            var config = Mock.Of<MyProducerConfiguration>();

            FluentActions.Invoking(() => { _ = new MyProducer<string, DummyProtobufForTest>(config); }).Should()
                         .ThrowExactly<ArgumentException>();

            config.WithSchemaRegistry(builder =>
            {
                builder.HavingUrl("https://localhost");
                builder.HavingAuthorizationMechanism(IMySchemaRegistryConfiguration.MyAuthorizationSource.UserInfo);
                builder.HavingCredentials("username", "password");
            });

            using var producer = new MyProducer<string, DummyProtobufForTest>(config);

            producer.Should().NotBeNull();
        }

        [Test]
        public void MyProducer_SchemaRegistry_AuthorizationMechanism_ShouldRequired_BootstrapServer_Credentials()
        {
            var config = Mock.Of<MyProducerConfiguration>();
            config.WithSchemaRegistry(builder =>
            {
                builder.HavingUrl("https://localhost");
                builder.HavingAuthorizationMechanism(IMySchemaRegistryConfiguration.MyAuthorizationSource.SaslInherit);
            });

            FluentActions.Invoking(() => { _ = new MyProducer<string, DummyProtobufForTest>(config); }).Should()
                         .ThrowExactly<ArgumentException>();

            config.WithBootstrapServer(builder =>
            {
                builder.HavingUrl("https://localhost");
                builder.HavingAuthorizationMechanism(IMyBootstrapServerConfiguration.MyAuthorizationMechanism.Plain);
                builder.HavingCredentials("username", "password");
            });

            using var producer = new MyProducer<string, DummyProtobufForTest>(config);

            producer.Should().NotBeNull();
        }

        [Test]
        public void MyProducer_ShouldReturnCreateMyProducerInstance()
        {
            var config = Mock.Of<MyProducerConfiguration>();
            config.WithClientId(Dns.GetHostName());
            config.WithAck(IMyProducerConfiguration.MyAck.Leader);
            config.WithMessageTimeout(TimeSpan.FromMilliseconds(5000));
            config.WithBootstrapServer(builder =>
            {
                builder.HavingUrl("https://localhost");
                builder.HavingAuthorizationMechanism(IMyBootstrapServerConfiguration.MyAuthorizationMechanism.Plain);
                builder.HavingCredentials("username", "password");
            });
            config.WithSchemaRegistry(builder =>
            {
                builder.HavingUrl("https://localhost");
                builder.HavingAuthorizationMechanism(IMySchemaRegistryConfiguration.MyAuthorizationSource.UserInfo);
                builder.HavingCredentials("username", "password");
            });

            using var producer = new MyProducer<string, DummyProtobufForTest>(config);

            producer.Should().NotBeNull();
        }
    }