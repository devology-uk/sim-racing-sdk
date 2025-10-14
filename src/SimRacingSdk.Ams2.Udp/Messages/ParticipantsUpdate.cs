#nullable disable

using SimRacingSdk.Ams2.Udp.Abstractions;

namespace SimRacingSdk.Ams2.Udp.Messages;

public record ParticipantsUpdate : MessageBase
{
    public ParticipantsUpdate(MessageHeader header)
        : base(header) { }

    public int ChangeTimestamp { get; internal set; }
    public string[] Names { get; internal set; }
    public int[] Nationalities { get; internal set; }
    public short[] Indexes { get; internal set; }
}