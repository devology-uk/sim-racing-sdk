namespace SimRacingSdk.Ace.Monitor.Abstractions;

public abstract record AceMonitorMessageBase
{
    public string Id { get; } = Guid.NewGuid()
                                    .ToString();
    public DateTime Timestamp { get; } = DateTime.UtcNow;
}
