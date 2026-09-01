using SimRacingSdk.Lmu.SharedMemory.Enums;
using SimRacingSdk.Lmu.SharedMemory.Messages;

namespace SimRacingSdk.Lmu.SharedMemory.Models;

public record LmuSharedMemorySession
{
    public LmuSharedMemorySession(LmuScoringInfo scoringInfo)
    {
        this.SessionId = Guid.NewGuid();
        this.SessionType = LmuSessionTypeResolver.Resolve(scoringInfo.Session);
        this.TrackName = scoringInfo.TrackName;
        this.MaxLaps = scoringInfo.MaxLaps;
        this.EndEt = scoringInfo.EndEt;
        this.NumberOfCars = Math.Clamp(scoringInfo.NumVehicles, 0, LmuSharedMemoryScoringData.MaxVehicles);
        this.IsRunning = true;
    }

    public double EndEt { get; }
    public bool IsRunning { get; internal set; }
    public int MaxLaps { get; }
    public int NumberOfCars { get; }
    public Guid SessionId { get; }
    public LmuSessionType SessionType { get; }
    public string TrackName { get; }
}
