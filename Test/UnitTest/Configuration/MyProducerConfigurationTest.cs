using FluentAssertions;

using MyKafkaClient.Core.Configuration;

using NUnit.Framework;

namespace KafkaClient.Test.UnitTest.Configuration;

[TestFixture]
public class MyProducerConfigurationTest
{
  [Test]
  public void Should_Require_At_Least_Topic()
  {
    MyProducerConfiguration configuration = new() { Topic = "topic" };

    configuration.Should().NotBeNull();

    configuration.GetConfiguration().Should().NotBeEmpty();
    configuration.GetConfiguration().Should().HaveCount(5);
    configuration.GetConfiguration().Should().BeOfType<List<KeyValuePair<string, string>>>();
    configuration.GetConfiguration().Should().BeAssignableTo<IEnumerable<KeyValuePair<string, string>>>();

    configuration.GetSchemaRegistry().Should().NotBeEmpty();
    configuration.GetSchemaRegistry().Should().HaveCount(1);
    configuration.GetSchemaRegistry().Should().BeOfType<List<KeyValuePair<string, string>>>();
    configuration.GetSchemaRegistry().Should().BeAssignableTo<IEnumerable<KeyValuePair<string, string>>>();
  }

  [Test]
  public void Should_Not_Accept_Null_Or_Empty_Topic()
  {
    FluentActions.Invoking(() => _ = new MyProducerConfiguration { Topic = string.Empty }).Should()
      .ThrowExactly<ArgumentNullException>();

    FluentActions.Invoking(() => _ = new MyProducerConfiguration { Topic = null }).Should()
      .ThrowExactly<ArgumentNullException>();
  }

  [Test]
  public void Should_Replace_Accept_Null_Or_Empty_Acknowledgement_With_Default_values()
  {
    FluentActions.Invoking(() =>
    {
      MyProducerConfiguration configuration = new();
      configuration.Acknowledgement.Should().Be(IMyProducerConfiguration.MyAcknowledgements.All);

      configuration = new MyProducerConfiguration { Acknowledgement = IMyProducerConfiguration.MyAcknowledgements.Leader };
      configuration.Acknowledgement.Should().Be(IMyProducerConfiguration.MyAcknowledgements.Leader);
    }).Should().NotThrow();
  }

  [Test]
  public void Should_Replace_Null_Or_Empty_EnableIdempotence_With_Default_values()
  {
    FluentActions.Invoking(() =>
    {
      MyProducerConfiguration configuration = new();
      configuration.EnableIdempotence.Should().BeFalse();

      configuration = new MyProducerConfiguration { EnableIdempotence = true };
      configuration.EnableIdempotence.Should().BeTrue();
    }).Should().NotThrow();
  }

  [Test]
  public void Should_Replace_Null_Or_Empty_EnableDeliveryReports_With_Default_values()
  {
    FluentActions.Invoking(() =>
    {
      MyProducerConfiguration configuration = new();
      configuration.EnableDeliveryReports.Should().BeTrue();

      configuration = new MyProducerConfiguration { EnableDeliveryReports = false };
      configuration.EnableDeliveryReports.Should().BeFalse();
    }).Should().NotThrow();
  }

  [Test]
  public void Should_Replace_Null_Or_Empty_Or_Less_Than_One_RetryBackoff_With_Default_values()
  {
    FluentActions.Invoking(() =>
    {
      MyProducerConfiguration configuration = new();
      configuration.RetryBackoff.Should().Be(1000);

      configuration = new MyProducerConfiguration { RetryBackoff = 500 };
      configuration.RetryBackoff.Should().Be(500);

      configuration = new MyProducerConfiguration { RetryBackoff = 0 };
      configuration.RetryBackoff.Should().Be(1000);
    }).Should().NotThrow();
  }

  [Test]
  public void Should_Replace_Null_Or_Empty_Or_Less_Than_One_MessageSendMaxRetries_With_Default_values()
  {
    FluentActions.Invoking(() =>
    {
      MyProducerConfiguration configuration = new();
      configuration.MessageSendMaxRetries.Should().Be(3);

      configuration = new MyProducerConfiguration { MessageSendMaxRetries = 5 };
      configuration.MessageSendMaxRetries.Should().Be(5);

      configuration = new MyProducerConfiguration { MessageSendMaxRetries = 0 };
      configuration.MessageSendMaxRetries.Should().Be(3);
    }).Should().NotThrow();
  }
}
