namespace SimRacingSdk.Acc.Monitor.Messages;

public record AccSessionStart(string EventId, string SessionType)
{
    public string Id { get; } = Guid.NewGuid()
                                    .ToString();
}