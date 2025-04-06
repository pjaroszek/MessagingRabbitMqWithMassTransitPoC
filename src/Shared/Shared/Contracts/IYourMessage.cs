namespace Jaroszek.CoderHouse.Shared.Contracts;

public interface IYourMessage
{
    Guid Id { get; }
    string Content { get; }
    DateTime Timestamp { get; }
}