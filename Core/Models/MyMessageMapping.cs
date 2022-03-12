using Confluent.Kafka;

namespace MyKafkaClient.Core.Models;

public static class MyMessageMapping
{
  public static Message<string, TValue> Map<TValue>(this MyMessage<TValue> source)
  {
    return new Message<string, TValue>
    {
      Key = source.Key,
      Value = source.Value,
      Timestamp = source.CreatedAt.Map(),
      Headers = source.Headers.Map()
    };
  }

  public static MyMessage<TValue> Map<TValue>(this Message<string, TValue> source)
  {
    return new MyMessage<TValue>(source.Key, source.Value, source.Headers?.Map(), source.Timestamp.Map());
  }

  private static Headers Map(this MyHeaderCollection collection)
  {
    Headers headers = new();
    collection.ToList().ForEach(each => headers.Add(each.Key, each.Value));
    return headers;
  }

  private static MyHeaderCollection Map(this Headers headers)
  {
    return new MyHeaderCollection(headers.ToDictionary(k => k.Key, v => v.GetValueBytes()));
  }

  private static Timestamp Map(this MyTimestamp timestamp)
  {
    return new Timestamp(timestamp.UtcDateTime);
  }

  private static MyTimestamp Map(this Timestamp timestamp)
  {
    return new MyTimestamp(timestamp.UtcDateTime);
  }

}
