namespace MyKafkaClient.Core.Models.Message ;

    public interface IMyMessage<out TKey, out TValue> : IMyMessageMetadata
    {
        public TKey Key { get; }
        public TValue Value { get; }
    }

    public sealed class MyMessage<TKey, TValue> : MyMessageMetadata, IMyMessage<TKey, TValue>
    {
        public MyMessage(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }

        public TKey Key { get; }
        public TValue Value { get; }
    }