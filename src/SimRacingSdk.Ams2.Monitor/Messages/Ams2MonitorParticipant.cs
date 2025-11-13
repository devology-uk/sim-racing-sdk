#nullable disable

using SimRacingSdk.Ams2.SharedMemory.Models;

namespace SimRacingSdk.Ams2.Monitor.Messages;

public record Ams2MonitorParticipant : Ams2Participant
{
    public Ams2MonitorParticipant(Ams2Participant ams2Participant)
    {
        this.BestSector1Time = ams2Participant.BestSector1Time;
        this.BestSector2Time = ams2Participant.BestSector2Time;
        this.BestSector3Time = ams2Participant.BestSector3Time;
        this.CarClassName = ams2Participant.CarClassName;
        this.CarName = ams2Participant.CarName;
        this.CurrentLap = ams2Participant.CurrentLap;
        this.CurrentSector = ams2Participant.CurrentSector;
        this.CurrentSector1Time = ams2Participant.CurrentSector1Time;
        this.CurrentSector2Time = ams2Participant.CurrentSector2Time;
        this.CurrentSector3Time = ams2Participant.CurrentSector3Time;
        this.DistanceIntoCurrentLap = ams2Participant.DistanceIntoCurrentLap;
        this.HighestFlagColor = ams2Participant.HighestFlagColor;
        this.HighestFlagReason = ams2Participant.HighestFlagReason;
        this.Index = ams2Participant.Index;
        this.IsActive = ams2Participant.IsActive;
        this.IsFocusedParticipant = ams2Participant.IsFocusedParticipant;
        this.IsLapInvalid = ams2Participant.IsLapInvalid;
        this.LapsCompleted = ams2Participant.LapsCompleted;
        this.LastLapTime = ams2Participant.LastLapTime;
        this.Name = ams2Participant.Name;
        this.Orientation = ams2Participant.Orientation;
        this.PitMode = ams2Participant.PitMode;
        this.PitSchedule = ams2Participant.PitSchedule;
        this.RacePosition = ams2Participant.RacePosition;
        this.RaceState = ams2Participant.RaceState;
        this.Speed = ams2Participant.Speed;
        this.WorldPosition = ams2Participant.WorldPosition;
    }

    public string CarManufacturer { get; init; }
    public string CountryCode { get; init; }
}