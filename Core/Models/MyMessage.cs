namespace MyKafkaClient.Core.Models;

public sealed class MyMessage<TValue> : MyMessageMetadata
{
  public MyMessage(string key, TValue value)
    : this(key, value, new MyHeaderCollection(), new MyTimestamp())
  {
    Key = key;
    Value = value;
  }

  public MyMessage(string key, TValue value, MyHeaderCollection? headers, MyTimestamp createdAt)
  : base(headers ?? new MyHeaderCollection(), createdAt)
  {
    ArgumentNullException.ThrowIfNull(key);
    ArgumentNullException.ThrowIfNull(value);

    Key = key;
    Value = value;
  }

  public string Key { get; }
  public TValue Value { get; }
}
