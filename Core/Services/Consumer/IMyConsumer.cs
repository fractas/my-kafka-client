using MyKafkaClient.Core.Models.Message;

namespace MyKafkaClient.Core.Services.Consumer ;

    public interface IMyConsumer<out TKey, out TValue> : IDisposable
    {
        void ConsumeAtMostOnce(string topic, Action<IMyMessage<TKey, TValue>> messageHandler);
        void ConsumeAtLeastOnce(string topic, Action<IMyMessage<TKey, TValue>> messageHandler);
        void Consume(string topic, Action<IMyMessage<TKey, TValue>> messageHandler);
    }