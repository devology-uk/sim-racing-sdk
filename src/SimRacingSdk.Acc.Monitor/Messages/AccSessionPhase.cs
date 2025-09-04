namespace SimRacingSdk.Acc.Monitor.Messages;

public record AccSessionPhase(string EventId, string SessionId, string Phase)
{
    public string Id { get; } = Guid.NewGuid()
                                    .ToString();
}