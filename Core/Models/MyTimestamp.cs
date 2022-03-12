namespace MyKafkaClient.Core.Models;

public sealed class MyTimestamp
{
  public MyTimestamp() : this(default)
  {
  }

  public MyTimestamp(DateTime? utc)
  {
    UtcDateTime = utc?.ToUniversalTime() ?? DateTime.UtcNow;
  }

  public DateTime UtcDateTime { get; }
}
