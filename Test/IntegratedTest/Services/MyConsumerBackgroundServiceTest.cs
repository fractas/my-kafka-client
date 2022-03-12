using FluentAssertions;

using KafkaClient.Test.IntegratedTest.Services;

using NUnit.Framework;

[TestFixture]
public class MyConsumerBackgroundServiceTest : MyServiceAbstractForTest
{
  [TearDown]
  public void TearDown()
  {
    _someBackgroundService.Dispose();
  }

  [Test]
  public void Should_Not_Throw_Exception_When_Starting()
  {
    FluentActions.Invoking(async () => await _someBackgroundService.StartAsync(new CancellationToken(true)))
      .Should().NotThrowAsync();
  }

  [Test]
  public void Should_Not_Throw_Exception_When_Stopping()
  {
    FluentActions.Invoking(async () => await _someBackgroundService.StopAsync(new CancellationToken(true)))
      .Should().NotThrowAsync();
  }

  [Test]
  public void Should_Throw_OperationCanceledException()
  {
    FluentActions.Invoking(async () =>
      {
        using CancellationTokenSource source = new(TimeSpan.FromSeconds(5));

        await _someBackgroundService.StartAsync(source.Token);
      })
      .Should().ThrowExactlyAsync<OperationCanceledException>();
  }
}
