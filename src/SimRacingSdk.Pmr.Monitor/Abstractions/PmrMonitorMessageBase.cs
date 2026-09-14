namespace SimRacingSdk.Pmr.Monitor.Abstractions;

public abstract record PmrMonitorMessageBase
{
    public string Id { get; } = Guid.NewGuid()
                                     .ToString();
    public DateTime Timestamp { get; } = DateTime.UtcNow;
}
