namespace SimRacingSdk.Acc.Monitor.Messages;

public record AccEvent(int TrackId, string TrackName, float TrackMeters)
{
    public string Id { get; } = Guid.NewGuid().ToString();
}