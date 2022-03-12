using FluentAssertions;

using MyKafkaClient.Core.Models;

using NUnit.Framework;

namespace KafkaClient.Test.UnitTest.Models;

[TestFixture]
public class MyMessageTest
{
  [Test]
  public void Should_Require_Minimum_Arguments()
  {
    const string key = "key1";
    const string value = "value";
    MyMessage<string> message = new(key, value);

    message.Should().NotBeNull();
    message.Key.Should().Be(key);
    message.Value.Should().Be(value);

    message.Headers.Should().BeEmpty();
    message.Headers.Should().HaveCount(0);

    message.CreatedAt.Should().NotBeNull();
    message.CreatedAt.UtcDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMilliseconds(10));
  }

  [Test]
  public void Should_All_Arguments_Constructor()
  {
    const string key = "key1";
    const string value = "value";

    DateTime utcDateTime = DateTime.UtcNow;
    MyTimestamp timestamp = new(utcDateTime);
    KeyValuePair<string, byte[]> header1 = new("key1", Array.Empty<byte>());
    MyHeaderCollection headers = new(header1);
    MyMessage<string> message = new(key, value, headers, timestamp);

    message.Should().NotBeNull();
    message.Key.Should().Be(key);
    message.Value.Should().Be(value);

    message.Headers.Should().NotBeEmpty();
    message.Headers.Should().HaveCount(1);

    message.CreatedAt.Should().NotBeNull();
    message.CreatedAt.UtcDateTime.Should().Be(utcDateTime);
  }
}
