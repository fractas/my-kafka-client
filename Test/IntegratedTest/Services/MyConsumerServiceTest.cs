using FluentAssertions;

using KafkaClient.Test.IntegratedTest.Services;

using MyKafkaClient.Core.Models;

using NUnit.Framework;

[TestFixture]
public class MyConsumerServiceTest : MyServiceAbstractForTest
{
  [SetUp]
  public void SetUp()
  {
    _someProducer.Produce("eproc-user-updated", new MyMessage<MyEntityCreated>(MyEntityCreated.MyEntity.Id, MyEntityCreated));
  }

  [Test]
  public void ShouldNotAdvanceOffsetWhenError()
  {
    FluentActions
      .Invoking(() => _someConsumer.Consume(_ => throw new InvalidOperationException(), CancellationToken.None))
      .Should()
      .NotThrow();

    FluentActions
      .Invoking(() => _someConsumer.Consume(message =>
        {
          message.Should().NotBeNull();

          message.Key.Should().NotBeNull();
          message.Value.Should().NotBeNull();
          message.Headers.Should().NotBeNull();
          message.CreatedAt.Should().NotBeNull();
        }, CancellationToken.None))
      .Should()
      .NotThrow();
  }
}

