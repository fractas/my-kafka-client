using Confluent.Kafka;

using FluentAssertions;

using MyKafkaClient.Core.Models;

using NUnit.Framework;

namespace KafkaClient.Test.UnitTest.Models;

[TestFixture]
public class MyMessageMappingTest
{
  [Test]
  public void Should_Map_From_MyMessageValue_To_MessageKeyValue()
  {
    Message<string, string> messageKeyValue = new()
    {
      Key = string.Empty,
      Value = string.Empty,
      Headers = new Headers { { "key1", Array.Empty<byte>() }, { "key2", Array.Empty<byte>() } },
      Timestamp = new Timestamp()
    };

    MyMessage<string> messageValue = messageKeyValue.Map();

    messageValue.Should().NotBeNull();
    messageValue.Should().BeOfType<MyMessage<string>>();

    messageKeyValue.Key.Should().Be(messageKeyValue.Key);
    messageKeyValue.Value.Should().Be(messageKeyValue.Value);
    messageKeyValue.Headers.Should().HaveSameCount(messageKeyValue.Headers);
    messageKeyValue.Timestamp.UtcDateTime.Should()
      .BeCloseTo(messageValue.CreatedAt.UtcDateTime, TimeSpan.FromMilliseconds(1));
  }

  [Test]
  public void Should_Map_From_MyMessageValue_To_MessageKeyValue_With_Minimum_Requirements()
  {
    Message<string, string> messageKeyValue = new() { Key = string.Empty, Value = string.Empty };

    messageKeyValue.Map().Should().BeOfType<MyMessage<string>>();
  }

  [Test]
  public void Should_Map_From_MessageKeyValue_To_MyMessageValue_With_Minimum_Requirements()
  {
    MyMessage<string> messageValue = new(string.Empty, string.Empty);

    messageValue.Map().Should().BeOfType<Message<string, string>>();
  }

  [Test]
  public void Should_Map_From_MessageKeyValue_To_MyMessageValue()
  {
    MyMessage<string> messageValue = new(
      string.Empty,
      string.Empty,
      new MyHeaderCollection { ["key1"] = Array.Empty<byte>(), ["key2"] = Array.Empty<byte>() },
      new MyTimestamp());

    Message<string, string> messageKeyValue = messageValue.Map();

    messageKeyValue.Should().NotBeNull();
    messageKeyValue.Should().BeOfType<Message<string, string>>();

    messageValue.Key.Should().Be(messageKeyValue.Key);
    messageValue.Value.Should().Be(messageKeyValue.Value);
    messageValue.Headers.Should().HaveSameCount(messageKeyValue.Headers);
    messageValue.CreatedAt.UtcDateTime.Should()
      .BeCloseTo(messageKeyValue.Timestamp.UtcDateTime, TimeSpan.FromMilliseconds(1));
  }
}
