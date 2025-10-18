using System.Numerics;
using SimRacingSdk.Ams2.SharedMemory.Messages;

namespace SimRacingSdk.Ams2.SharedMemory.Models;

public record Ams2ParticipantInfo
{
    internal Ams2ParticipantInfo(ParticipantInfo participantInfo)
    {
        this.IsActive = participantInfo.IsActive;
        this.Name = participantInfo.Name;
        this.WorldPosition = new Vector3(participantInfo.WorldPosition[0],
            participantInfo.WorldPosition[1],
            participantInfo.WorldPosition[2]);
        this.DistanceIntoCurrentLap = participantInfo.DistanceIntoCurrentLap;
        this.RacePosition = participantInfo.RacePosition;
        this.LapsCompleted = participantInfo.LapsCompleted;
        this.CurrentLap = participantInfo.CurrentLap;
        this.CurrentSector = participantInfo.CurrentSector;
    }

    public uint CurrentLap { get; init; }
    public int CurrentSector { get; init; }
    public float DistanceIntoCurrentLap { get; init; }
    public bool IsActive { get; init; }
    public uint LapsCompleted { get; init; }
    public string Name { get; init; }
    public uint RacePosition { get; init; }
    public Vector3 WorldPosition { get; init; }
}