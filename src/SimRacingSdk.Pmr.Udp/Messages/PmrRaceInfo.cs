using SimRacingSdk.Pmr.Udp.Enums;

namespace SimRacingSdk.Pmr.Udp.Messages;

public record PmrRaceInfo
{
    public ushort PacketVersion { get; init; }
    public string Track { get; init; } = string.Empty;
    public string Layout { get; init; } = string.Empty;
    public string Season { get; init; } = string.Empty;
    public string Weather { get; init; } = string.Empty;
    public string Session { get; init; } = string.Empty;
    public string GameMode { get; init; } = string.Empty;
    public float LayoutLength { get; init; }
    public float Duration { get; init; }
    public float Overtime { get; init; }
    public float AmbientTemperature { get; init; }
    public float TrackTemperature { get; init; }
    public bool IsLaps { get; init; }
    public PmrRaceSessionState State { get; init; }
    public byte NumberOfParticipants { get; init; }
}
