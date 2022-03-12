using FluentAssertions;

using MyKafkaClient.Core.Models;

using NUnit.Framework;

namespace KafkaClient.Test.UnitTest.Models;

[TestFixture]
public class MyTimestampTest
{
  [Test]
  public void Should_Not_Require_Any_Arguments()
  {
    MyTimestamp timestamp = new();

    timestamp.Should().NotBeNull();
    timestamp.UtcDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMilliseconds(10));
  }

  [Test]
  public void Should_Receive_LocalDateTime_And_Return_UtcDateTime()
  {
    MyTimestamp timestamp = new(DateTime.Now);

    timestamp.Should().NotBeNull();
    timestamp.UtcDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMilliseconds(10));
  }

  [Test]
  public void Should_Be_Different_From_LocalDateTime()
  {
    DateTime datetime = DateTime.Now;
    MyTimestamp timestamp = new(datetime);

    timestamp.Should().NotBeNull();
    timestamp.UtcDateTime.Should().NotBe(datetime);
  }

  [Test]
  public void Should_Receive_Null_And_Return_Current_UtcDateTime()
  {
    DateTime? datetime = null;
    MyTimestamp timestamp = new(datetime);

    timestamp.Should().NotBeNull();
    timestamp.UtcDateTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMilliseconds(10));
  }
}
