using FluentAssertions;

using MyKafkaClient.Core.Configuration;

using NUnit.Framework;

namespace KafkaClient.Test.UnitTest.Configuration;

[TestFixture]
public class MyConsumerConfigurationTest
{
  [Test]
  public void Should_Require_At_Least_Topic_Ang_GroupId()
  {
    MyConsumerConfiguration configuration = new() { Topic = "topic", GroupId = "group" };

    configuration.Should().NotBeNull();

    configuration.GetConfiguration().Should().NotBeEmpty();
    configuration.GetConfiguration().Should().HaveCount(5);
    configuration.GetConfiguration().Should().BeOfType<List<KeyValuePair<string, string>>>();
    configuration.GetConfiguration().Should().BeAssignableTo<IEnumerable<KeyValuePair<string, string>>>();
  }

  [Test]
  public void Should_Not_Accept_Null_Or_Empty_Topic()
  {
    FluentActions.Invoking(() => _ = new MyConsumerConfiguration { Topic = string.Empty }).Should()
      .ThrowExactly<ArgumentNullException>();

    FluentActions.Invoking(() => _ = new MyConsumerConfiguration { Topic = null }).Should()
      .ThrowExactly<ArgumentNullException>();
  }

  [Test]
  public void Should_Not_Accept_Null_Or_Empty_GroupId()
  {
    FluentActions.Invoking(() => _ = new MyConsumerConfiguration { GroupId = string.Empty }).Should()
      .ThrowExactly<ArgumentNullException>();

    FluentActions.Invoking(() => _ = new MyConsumerConfiguration { GroupId = null }).Should()
      .ThrowExactly<ArgumentNullException>();
  }

  [Test]
  public void Should_Replace_Accept_Null_Or_Empty_AutoOffsetReset_With_Default_values()
  {
    FluentActions.Invoking(() =>
    {
      MyConsumerConfiguration configuration = new();
      configuration.AutoOffsetReset.Should().Be(IMyConsumerConfiguration.MyOffsetReset.Earliest);

      configuration = new MyConsumerConfiguration { AutoOffsetReset = IMyConsumerConfiguration.MyOffsetReset.Latest };
      configuration.AutoOffsetReset.Should().Be(IMyConsumerConfiguration.MyOffsetReset.Latest);
    }).Should().NotThrow();
  }

  [Test]
  public void Should_Replace_Null_Or_Empty_Or_Less_Than_One_MaxPollInterval_With_Default_values()
  {
    FluentActions.Invoking(() =>
    {
      MyConsumerConfiguration configuration = new();
      configuration.MaxPollInterval.Should().Be(250000);

      configuration = new MyConsumerConfiguration { MaxPollInterval = 5000 };
      configuration.MaxPollInterval.Should().Be(5000);

      configuration = new MyConsumerConfiguration { MaxPollInterval = 0 };
      configuration.MaxPollInterval.Should().Be(250000);
    }).Should().NotThrow();
  }

  [Test]
  public void Should_Replace_Null_Or_Empty_AutoCommit_With_Default_values()
  {
    FluentActions.Invoking(() =>
    {
      MyConsumerConfiguration configuration = new();
      configuration.AutoCommit.Should().BeFalse();

      configuration = new MyConsumerConfiguration { AutoCommit = true };
      configuration.AutoCommit.Should().BeTrue();
    }).Should().NotThrow();
  }

  [Test]
  public void Should_Replace_Null_Or_Empty_AutoOffsetStore_With_Default_values()
  {
    FluentActions.Invoking(() =>
    {
      MyConsumerConfiguration configuration = new();
      configuration.AutoOffsetStore.Should().BeFalse();

      configuration = new MyConsumerConfiguration { AutoOffsetStore = true };
      configuration.AutoOffsetStore.Should().BeTrue();
    }).Should().NotThrow();
  }
}
