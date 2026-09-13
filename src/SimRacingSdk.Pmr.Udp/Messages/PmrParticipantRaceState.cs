using SimRacingSdk.Pmr.Udp.Enums;

namespace SimRacingSdk.Pmr.Udp.Messages;

public record PmrParticipantRaceState
{
    public ushort PacketVersion { get; init; }
    public int VehicleId { get; init; }
    public bool IsPlayer { get; init; }
    public string VehicleName { get; init; } = string.Empty;
    public string DriverName { get; init; } = string.Empty;
    public string LiveryId { get; init; } = string.Empty;
    public string VehicleClass { get; init; } = string.Empty;
    public int RacePosition { get; init; }
    public int CurrentLap { get; init; }
    public TimeSpan CurrentLapTime { get; init; }
    public TimeSpan BestLapTime { get; init; }
    public float LapProgress { get; init; }
    public int CurrentSector { get; init; }
    public IReadOnlyList<TimeSpan> CurrentSectorTimes { get; init; } = [];
    public IReadOnlyList<TimeSpan> BestSectorTimes { get; init; } = [];
    public bool InPits { get; init; }
    public bool SessionFinished { get; init; }
    public bool IsDisqualified { get; init; }
    public PmrRaceFlags Flags { get; init; }
}
