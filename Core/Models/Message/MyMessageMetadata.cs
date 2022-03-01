namespace MyKafkaClient.Core.Models.Message;

public interface IMyMessageMetadata
{
    public IMyTimestamp CreatedAt { get; }
    public IMyHeaderCollection Headers { get; }
}

public abstract class MyMessageMetadata : IMyMessageMetadata
{
    public IMyTimestamp CreatedAt { get; set; } = new MyTimestamp();
    public IMyHeaderCollection Headers { get; set; } = new MyHeaderCollection();
}