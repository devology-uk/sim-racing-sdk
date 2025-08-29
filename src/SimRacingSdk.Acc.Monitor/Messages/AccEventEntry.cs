#nullable disable

using SimRacingSdk.Acc.Udp.Messages;

namespace SimRacingSdk.Acc.Monitor.Messages;

public record AccEventEntry()
{
    public string Id { get; } = Guid.NewGuid()
                                    .ToString();
    public CarInfo Car { get; init; }
    public DriverInfo CurrentDriver { get; init; }
    public string EventId { get; init; }
}