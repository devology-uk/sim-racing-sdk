namespace SimRacingSdk.Lmu.Monitor.Abstractions;

public abstract record LmuMonitorMessageBase
{
    public string Id { get; } = Guid.NewGuid()
                                    .ToString();
    public DateTime Timestamp { get; } = DateTime.UtcNow;
}
