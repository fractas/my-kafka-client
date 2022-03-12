using FluentAssertions;

using KafkaClient.Test.IntegratedTest.Services;

using MyKafkaClient.Core.Models;

using NUnit.Framework;

[TestFixture]
public class MyProducerServiceTest : MyServiceAbstractForTest
{
  [SetUp]
  public void SetUp()
  {
    _someProducer.Produce("my-entity-created", new MyMessage<MyEntityCreated>(MyEntityCreated.MyEntity.Id, MyEntityCreated));
  }

  [Test]
  public void Should_Not_Throw_When_ExecuteAsync()
  {
    FluentActions
      .Invoking(() => _someConsumer.Consume(message => message.Value.Should().NotBeNull(), CancellationToken.None))
      .Should()
      .NotThrow();
  }
}

