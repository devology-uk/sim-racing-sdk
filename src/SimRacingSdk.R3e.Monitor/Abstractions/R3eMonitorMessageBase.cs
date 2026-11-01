namespace SimRacingSdk.R3e.Monitor.Abstractions;

public abstract record R3eMonitorMessageBase
{
    public string Id { get; } = Guid.NewGuid()
                                    .ToString();
    public DateTime Timestamp { get; } = DateTime.UtcNow;
}
