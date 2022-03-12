using Google.Protobuf;

using MyKafkaClient.Core.Models;

namespace MyKafkaClient.Core.Interfaces;

public interface IMyConsumerHandler<TMessage> where TMessage : class, IMessage<TMessage>, new()
{
  void Handle(MyMessage<TMessage> message);
}
