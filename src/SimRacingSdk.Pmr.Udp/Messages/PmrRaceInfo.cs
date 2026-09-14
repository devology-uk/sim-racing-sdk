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
    public float LayoutLengthMeters { get; init; }

    // Seconds is a best guess, not empirically confirmed against a known session length - a
    // 2026-09-13 rig capture showed 300 for a race weekend's race session, which reads more
    // plausibly as seconds (5 minutes) for a short/quick race than any other unit tried.
    public float DurationSeconds { get; init; }

    public float OvertimeSeconds { get; init; }
    public float AmbientTemperatureCelsius { get; init; }
    public float TrackTemperatureCelsius { get; init; }
    public bool IsLaps { get; init; }
    public PmrRaceSessionState State { get; init; }
    public byte NumberOfParticipants { get; init; }
}
