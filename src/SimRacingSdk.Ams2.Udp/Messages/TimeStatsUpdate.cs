#nullable disable

using SimRacingSdk.Ams2.Udp.Abstractions;

namespace SimRacingSdk.Ams2.Udp.Messages;

public record TimeStatsUpdate : MessageBase
{
    public TimeStatsUpdate(MessageHeader header)
        : base(header) { }

    public int ChangeTimestamp { get; internal set; }
    public ParticipantStats[] ParticipantStats { get; internal set; }
}