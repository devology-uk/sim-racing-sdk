using SimRacingSdk.Lmu.SharedMemory.Messages;

namespace SimRacingSdk.Lmu.SharedMemory.Models;

public record LmuSharedMemoryLap
{
    public LmuSharedMemoryLap(LmuVehicleScoring scoring, Guid sessionId, string trackName)
    {
        this.SessionId = sessionId;
        this.DriverName = scoring.DriverName;
        this.VehicleName = scoring.VehicleName;
        this.CompletedLaps = scoring.TotalLaps;
        this.LastLapTimeMs = ToMilliseconds(scoring.LastLapTime);
        this.Sector1Ms = ToMilliseconds(scoring.LastSector1);
        this.Sector2Ms = ToMilliseconds(scoring.LastSector2 - scoring.LastSector1);
        this.Sector3Ms = ToMilliseconds(scoring.LastLapTime - scoring.LastSector2);
        this.TrackName = trackName;
    }

    public int CompletedLaps { get; }
    public string DriverName { get; }
    public int LastLapTimeMs { get; }
    public Guid SessionId { get; }

    // LastSector1/LastSector2 are cumulative-from-lap-start, matching the sector convention already used elsewhere
    // in this SDK (Acc/Ams2) - not yet confirmed against a real multi-sector lap on Mike's rig.
    public int Sector1Ms { get; }
    public int Sector2Ms { get; }
    public int Sector3Ms { get; }
    public string TrackName { get; }
    public string VehicleName { get; }

    private static int ToMilliseconds(double seconds)
    {
        return (int)Math.Round(seconds * 1000);
    }
}
