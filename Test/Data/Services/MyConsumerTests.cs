using System.Net;

using FluentAssertions;

using MyKafkaClient.Core.Services.Configuration.BootstrapServer;
using MyKafkaClient.Core.Services.Consumer;
using MyKafkaClient.Data.Services.Consumer;

using NUnit.Framework;

namespace MyKafkaClient.Test.Data.Services ;

    public class MyConsumerTests
    {
        [Test]
        public void MyConsumer_GroupId_ShouldBeRequired()
        {
            var config = new MyConsumerConfiguration();

            FluentActions.Invoking(() => { _ = new MyConsumer<string, DummyProtobufForTest>(config); }).Should()
                         .ThrowExactly<ArgumentException>();

            config.WithGroupId("GroupId");

            using var consumer = new MyConsumer<string, DummyProtobufForTest>(config);

            consumer.Should().NotBeNull();
        }

        [Test]
        public void MyConsumer_MaxPollInterval_ShouldBeEqualOrGreaterThanSessionTimeout()
        {
            var config = new MyConsumerConfiguration();
            config.WithClientId(Dns.GetHostName());
            config.WithGroupId("GroupId");
            config.WithAck(IMyConsumerConfiguration.MyAck.Leader);
            config.WithAutoCommit();
            config.WithPartitionEof(true);
            config.WithAutoCommitInterval(TimeSpan.FromMilliseconds(1));
            config.WithAutoCreateTopics(true);
            config.WithAutoOffsetReset(IMyConsumerConfiguration.MyOffsetReset.Latest);
            config.WithAutoOffsetStore(true);
            config.WithSessionTimeout(TimeSpan.FromMilliseconds(4));
            config.WithMaxPollInterval(TimeSpan.FromMilliseconds(3));
            config.WithMetadataMaxAge(TimeSpan.FromMilliseconds(1));
            config.WithBootstrapServer(builder =>
            {
                builder.HavingUrl("https://localhost");
                builder.HavingAuthorizationMechanism(IMyBootstrapServerConfiguration.MyAuthorizationMechanism.Plain);
                builder.HavingCredentials("username", "password");
            });

            FluentActions.Invoking(() => { _ = new MyConsumer<string, DummyProtobufForTest>(config); }).Should()
                         .ThrowExactly<InvalidOperationException>();

            config.WithMaxPollInterval(TimeSpan.FromMilliseconds(5));

            using var consumer = new MyConsumer<string, DummyProtobufForTest>(config);

            consumer.Should().NotBeNull();
        }
    }