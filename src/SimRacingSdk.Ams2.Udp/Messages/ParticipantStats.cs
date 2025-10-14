#nullable disable

namespace SimRacingSdk.Ams2.Udp.Messages;

public record ParticipantStats
{
    public int BestLapTime { get; internal set; }
    public int BestSector1 { get; internal set; }
    public int BestSector2 { get; internal set; }
    public int BestSector3 { get; internal set; }
    public int LastLapTime { get; internal set; }
    public int LastSectorTime { get; internal set; }
    public int OnlineRep { get; internal set; }
    public short ParticipantIndex { get; internal set; }
}