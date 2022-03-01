using Confluent.Kafka;

using MyKafkaClient.Core.Models.Message;

namespace MyKafkaClient.Core.Mappings;

public static class MessageMappingsExtensions
{
    public static IMyMessage<TKey, TValue> Map<TKey, TValue>(this Message<TKey, TValue> source)
    {
        return new MyMessage<TKey, TValue>(source.Key, source.Value)
        {
            CreatedAt = new MyTimestamp(source.Timestamp.UtcDateTime),
            Headers = new MyHeaderCollection(source.Headers.ToDictionary(k => k.Key, v => v.GetValueBytes()))
        };
    }

    public static Message<TKey, TValue> Map<TKey, TValue>(this IMyMessage<TKey, TValue> source)
    {
        Message<TKey, TValue> destination = new()
        {
            Key = source.Key,
            Value = source.Value,
            Timestamp = new Timestamp(source.CreatedAt.UtcDateTime),
            Headers = new Headers()
        };

        source.Headers.ToList().ForEach(each => destination.Headers.Add(each.Key, each.Value));
        return destination;
    }
}