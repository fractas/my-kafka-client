namespace MyKafkaClient.Core.Models.Message;

public interface IMyTimestamp
{
    DateTime UtcDateTime { get; }
}

public sealed class MyTimestamp : IMyTimestamp
{
    public MyTimestamp() : this(DateTime.UtcNow)
    {
    }

    public MyTimestamp(DateTime utc)
    {
        ArgumentNullException.ThrowIfNull(utc);

        UtcDateTime = utc.ToUniversalTime();
    }

    public DateTime UtcDateTime { get; }
}