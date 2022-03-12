using FluentAssertions;

using MyKafkaClient.Core.Interfaces;
using MyKafkaClient.Core.Models;

namespace KafkaClient.Test.Common;

public class MyEntityCreatedHandlerForTest : IMyConsumerHandler<MyEntityCreated>
{
  private readonly IDictionary<string, MyEntityCreated> _items = new Dictionary<string, MyEntityCreated>();

  public IEnumerable<MyEntityCreated> Items => _items.Values.ToList();

  public void Handle(MyMessage<MyEntityCreated> message)
  {
    message.Should().NotBeNull();
    message.Key.Should().NotBeNull();
    message.Value.Should().NotBeNull();
    message.Value.MyEntity.Should().NotBeNull();
    message.Value.MyEntity.Id.Should().NotBeNull();
    message.Value.MyEntity.Id.Should().Be(message.Key);

    _items.Add(message.Value.MyEntity.Id, message.Value);
  }
}
