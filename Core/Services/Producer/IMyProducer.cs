using MyKafkaClient.Core.Models.Message;

namespace MyKafkaClient.Core.Services.Producer ;

    public interface IMyProducer<in TKey, in TValue> : IDisposable
    {
        void ProduceAndConfirm(string topic, IMyMessage<TKey, TValue> message);
        void Produce(string topic, IMyMessage<TKey, TValue> message);
    }