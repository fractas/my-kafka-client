namespace MyKafkaClient.Core.Models;

public abstract class MyMessageMetadata
{
  public MyMessageMetadata(MyHeaderCollection headers, MyTimestamp createdAt)
  {
    ArgumentNullException.ThrowIfNull(headers);
    ArgumentNullException.ThrowIfNull(createdAt);

    Headers = headers;
    CreatedAt = createdAt;
  }

  public MyTimestamp CreatedAt { get; init; }
  public MyHeaderCollection Headers { get; init; }
}
