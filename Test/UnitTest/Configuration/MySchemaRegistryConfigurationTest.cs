using FluentAssertions;

using MyKafkaClient.Core.Configuration;

using NUnit.Framework;

namespace KafkaClient.Test.UnitTest.Configuration;

[TestFixture]
public class MySchemaRegistryConfigurationTest
{
  [Test]
  public void Should_Not_Require_Any_Arguments()
  {
    MySchemaRegistryConfiguration configuration = new();

    configuration.Should().NotBeNull();
    configuration.GetConfiguration().Should().NotBeEmpty();
    configuration.GetConfiguration().Should().HaveCount(1);
    configuration.GetConfiguration().Should().BeOfType<List<KeyValuePair<string, string>>>();
    configuration.GetConfiguration().Should().BeAssignableTo<IEnumerable<KeyValuePair<string, string>>>();
  }

  [Test]
  public void Should_Not_Accept_Null_Or_Empty_Url()
  {
    FluentActions.Invoking(() => _ = new MySchemaRegistryConfiguration { Url = string.Empty }).Should()
      .ThrowExactly<ArgumentNullException>();

    FluentActions.Invoking(() => _ = new MySchemaRegistryConfiguration { Url = null }).Should()
      .ThrowExactly<ArgumentNullException>();
  }

  [Test]
  public void Should_Accept_Null_Or_Empty_Credential()
  {
    FluentActions.Invoking(() => _ = new MySchemaRegistryConfiguration { Credential = string.Empty }).Should()
      .NotThrow();

    FluentActions.Invoking(() => _ = new MySchemaRegistryConfiguration { Credential = null }).Should()
      .NotThrow();
  }
}
