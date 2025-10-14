#nullable disable

using SimRacingSdk.Ams2.Udp.Abstractions;

namespace SimRacingSdk.Ams2.Udp.Messages;

public record TimingsUpdate : MessageBase
{
    public TimingsUpdate(MessageHeader header)
        : base(header) { }

    public int ChangeTimeStamp { get; internal set; }
    public int EventTimeRemaining { get; internal set; }
    public short LocalParticipantIndex { get; internal set; }
    public byte ParticipantCount { get; internal set; }
    public ParticipantInfo[] Participants { get; internal set; }
    public int SplitTime { get; internal set; }
    public int SplitTimeAhead { get; internal set; }
    public int SplitTimeBehind { get; internal set; }
    public int TickCount { get; internal set; }
}