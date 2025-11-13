#nullable disable

namespace SimRacingSdk.Ams2.SharedMemory.Models;

public record Ams2Lap
{
    public TimeSpan BestSector1Time { get; init; }
    public TimeSpan BestSector2Time { get; init; }
    public TimeSpan BestSector3Time { get; init; }
    public string CarClassName { get; init; }
    public string CarName { get; init; }
    public bool IsPlayerLap { get; init; }
    public bool IsValid { get; init; }
    public uint Lap { get; init; }
    public TimeSpan LapTime { get; init; }
    public int ParticipantIndex { get; init; }
    public string ParticipantName { get; init; }
    public uint ParticipantNationality { get; init; }
    public TimeSpan Sector1Time { get; init; }
    public TimeSpan Sector2Time { get; init; }
    public TimeSpan Sector3Time { get; init; }
}