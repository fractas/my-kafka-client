using System.Text;

using Confluent.Kafka;

using FluentAssertions;

using MyKafkaClient.Core.Mappings;
using MyKafkaClient.Core.Models.Message;

using NUnit.Framework;

namespace MyKafkaClient.Test.Mappings;

public class MappingTests
{
    [Test]
    public void Map_ShouldMapToMessage()
    {
        MyMessageForTest myMessage = new();

        Message<string, DummyProtobufForTest> message = myMessage.Map();

        message.Should().NotBeNull();
        message.Key.Should().BeSameAs(myMessage.Key);
        message.Value.Should().BeSameAs(myMessage.Value);
        message.Timestamp.Should().NotBeNull();
        message.Timestamp.UtcDateTime.Should().BeSameDateAs(myMessage.CreatedAt.UtcDateTime);
        message.Headers.Should().HaveSameCount(myMessage.Headers);
        message.Headers[0].GetValueBytes().Should().BeSameAs(myMessage.Headers["x-my-header-test"]);
    }

    [Test]
    public void Map_ShouldMapToMyMessage()
    {
        MessageForTest message = new();

        IMyMessage<string, DummyProtobufForTest> myMessage = message.Map();

        myMessage.Should().NotBeNull();
        myMessage.Key.Should().BeSameAs(message.Key);
        myMessage.Value.Should().BeSameAs(message.Value);
        myMessage.CreatedAt.Should().NotBeNull();
        myMessage.CreatedAt.UtcDateTime.Should().BeSameDateAs(message.Timestamp.UtcDateTime);
        myMessage.Headers.Should().HaveSameCount(message.Headers);
        myMessage.Headers["x-header-test"].Should().BeSameAs(message.Headers.First().GetValueBytes());
    }

    private class MyMessageForTest : IMyMessage<string, DummyProtobufForTest>
    {
        public MyMessageForTest()
        {
            CreatedAt = new MyTimestamp();
            Headers =
                new MyHeaderCollection(new KeyValuePair<string, byte[]>("x-my-header-test",
                    Encoding.UTF8.GetBytes("x-my-test-value")));
            Key = Guid.NewGuid().ToString();
            Value = new DummyProtobufForTest();
        }

        public IMyTimestamp CreatedAt { get; }
        public IMyHeaderCollection Headers { get; }
        public string Key { get; }
        public DummyProtobufForTest Value { get; }
    }

    private class MessageForTest : Message<string, DummyProtobufForTest>
    {
        public MessageForTest()
        {
            Key = Guid.NewGuid().ToString();
            Value = new DummyProtobufForTest();
            Timestamp = new Timestamp(DateTime.UtcNow);
            Headers = new Headers { { "x-header-test", Encoding.UTF8.GetBytes("x-test-value") } };
        }
    }
}