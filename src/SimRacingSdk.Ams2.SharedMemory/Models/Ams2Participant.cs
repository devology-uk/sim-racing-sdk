#nullable disable

using System.Numerics;
using SimRacingSdk.Ams2.SharedMemory.Enums;

namespace SimRacingSdk.Ams2.SharedMemory.Models;

public record Ams2Participant
{
    public TimeSpan BestSector1Time { get; init; }
    public TimeSpan BestSector2Time { get; init; }
    public TimeSpan BestSector3Time { get; init; }
    public string CarClassName { get; init; }
    public string CarName { get; init; }
    public uint CurrentLap { get; init; }
    public int CurrentSector { get; init; }
    public TimeSpan CurrentSector1Time { get; init; }
    public TimeSpan CurrentSector2Time { get; init; }
    public TimeSpan CurrentSector3Time { get; init; }
    public float DistanceIntoCurrentLap { get; init; }
    public Ams2FlagColor HighestFlagColor { get; init; }
    public Ams2FlagReason HighestFlagReason { get; init; }
    public int Index { get; set; }
    public bool IsActive { get; init; }
    public bool IsFocusedParticipant { get; set; }
    public bool IsLapInvalid { get; init; }
    public uint LapsCompleted { get; init; }
    public TimeSpan LastLapTime { get; init; }
    public string Name { get; init; }
    public uint Nationality { get; init; }
    public Vector3 Orientation { get; init; }
    public Ams2PitMode PitMode { get; init; }
    public Ams2PitStopSchedule PitSchedule { get; init; }
    public uint RacePosition { get; init; }
    public Ams2RaceState RaceState { get; init; }
    public float Speed { get; init; }
    public Vector3 WorldPosition { get; init; }
}