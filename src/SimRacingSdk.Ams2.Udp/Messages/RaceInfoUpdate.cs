#nullable disable

using SimRacingSdk.Ams2.Udp.Abstractions;

namespace SimRacingSdk.Ams2.Udp.Messages;

public record RaceInfoUpdate : MessageBase
{
    public RaceInfoUpdate(MessageHeader header)
        : base(header) { }

    public byte IsPitStopMandatory { get; internal set; }
    public bool IsTimedEvent { get; internal set; }
    public int OverallBestLapTime { get; internal set; }
    public int OverallBestSector1 { get; internal set; }
    public int OverallBestSector2 { get; internal set; }
    public int OverallBestSector3 { get; internal set; }
    public int PersonalBestLapTime { get; internal set; }
    public int PersonalBestSector1 { get; internal set; }
    public int PersonalBestSector2 { get; internal set; }
    public int PersonalBestSector3 { get; internal set; }
    public int ScheduledLaps { get; internal set; }
    public TimeSpan ScheduledTime { get; internal set; }
    public string TrackLayout { get; internal set; }
    public int TrackLength { get; internal set; }
    public string TrackLocation { get; internal set; }
    public string TranslatedTrackLayout { get; internal set; }
    public string TranslatedTrackLocation { get; internal set; }
}