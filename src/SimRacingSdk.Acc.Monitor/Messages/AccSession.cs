#nullable disable

namespace SimRacingSdk.Acc.Monitor.Messages;

public record AccSession(string EventId, string SessionType)
{
    public string Id { get; } = Guid.NewGuid()
                                    .ToString();

}