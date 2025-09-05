namespace SimRacingSdk.Acc.Monitor.Abstractions;

public abstract record AccMonitorMessageBase
{
    public string Id { get; } = Guid.NewGuid()
                                    .ToString();
    public DateTime Timestamp { get; } = DateTime.UtcNow;
}