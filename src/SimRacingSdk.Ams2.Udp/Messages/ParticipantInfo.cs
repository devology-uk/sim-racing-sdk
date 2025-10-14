#nullable disable

namespace SimRacingSdk.Ams2.Udp.Messages;

public record ParticipantInfo
{
    public short CarIndex { get; internal set; }
    public byte CurrentLap { get; internal set; }
    public short CurrentLapDistance { get; internal set; }
    public int CurrentTime { get; internal set; }
    public int FlagColour { get; internal set; }
    public int FlagReason { get; internal set; }
    public bool IsActive { get; internal set; }
    public bool IsLapInvalid { get; internal set; }
    public short[] Orientation { get; internal set; }
    public short ParticipantIndex { get; internal set; }
    public int PitMode { get; internal set; }
    public int PitSchedule { get; internal set; }
    public int Position { get; internal set; }
    public int RaceStateFlags { get; internal set; }
    public int Sector { get; internal set; }
    public short[] WorldPosition { get; internal set; }
    public int XExtraPrecision { get; internal set; }
    public int ZExtraPrecision { get; internal set; }
}