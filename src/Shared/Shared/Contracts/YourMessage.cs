namespace Jaroszek.CoderHouse.Shared.Contracts;

public class YourMessage: IYourMessage
{
    public YourMessage(string message)
    {
        Id = Guid.NewGuid();
        Content = message;
        Timestamp = DateTime.UtcNow;
    }
    
    public Guid Id { get; }
    public string Content { get; }
    public DateTime Timestamp { get; }
}