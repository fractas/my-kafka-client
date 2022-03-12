using FluentAssertions;

using MyKafkaClient.Core.Configuration;

using NUnit.Framework;

namespace KafkaClient.Test.UnitTest.Configuration;

[TestFixture]
public class MyBootstrapServerConfigurationTest
{
  [Test]
  public void Should_Not_Require_Any_Arguments()
  {
    MyBootstrapServerConfiguration configuration = new();

    configuration.Should().NotBeNull();
    configuration.GetConfiguration().Should().NotBeEmpty();
    configuration.GetConfiguration().Should().HaveCount(3);
    configuration.GetConfiguration().Should().BeOfType<List<KeyValuePair<string, string>>>();
    configuration.GetConfiguration().Should().BeAssignableTo<IEnumerable<KeyValuePair<string, string>>>();
  }

  [Test]
  public void Should_Not_Accept_Null_Or_Empty_Url()
  {
    FluentActions.Invoking(() => _ = new MyBootstrapServerConfiguration { Url = string.Empty }).Should()
      .ThrowExactly<ArgumentNullException>();

    FluentActions.Invoking(() => _ = new MyBootstrapServerConfiguration { Url = null }).Should()
      .ThrowExactly<ArgumentNullException>();
  }


  [Test]
  public void Should_Accept_Null_Or_Empty_Password()
  {
    FluentActions.Invoking(() => _ = new MyBootstrapServerConfiguration { Password = string.Empty }).Should()
      .NotThrow();

    FluentActions.Invoking(() => _ = new MyBootstrapServerConfiguration { Password = null }).Should()
      .NotThrow();
  }


  [Test]
  public void Should_Accept_Null_Or_Empty_Username()
  {
    FluentActions.Invoking(() => _ = new MyBootstrapServerConfiguration { Username = string.Empty }).Should()
      .NotThrow();

    FluentActions.Invoking(() => _ = new MyBootstrapServerConfiguration { Username = null }).Should()
      .NotThrow();
  }
}
