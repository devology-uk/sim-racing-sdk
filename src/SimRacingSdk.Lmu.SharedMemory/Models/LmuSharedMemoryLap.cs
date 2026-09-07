using SimRacingSdk.Lmu.SharedMemory.Enums;
using SimRacingSdk.Lmu.SharedMemory.Messages;

namespace SimRacingSdk.Lmu.SharedMemory.Models;

public record LmuSharedMemoryLap
{
    public LmuSharedMemoryLap(LmuVehicleScoring scoring, LmuVehicleTelemetry telemetry, Guid sessionId,
        string trackName)
    {
        this.SessionId = sessionId;
        this.DriverName = scoring.DriverName;
        this.VehicleName = scoring.VehicleName;
        this.VehicleModel = telemetry.VehicleModel;
        this.VehicleClass = telemetry.VehicleClass;
        this.VehicleClassName = scoring.VehicleClass;
        this.CompletedLaps = scoring.TotalLaps;
        this.LastLapTimeMs = ToMilliseconds(scoring.LastLapTime);
        this.Sector1Ms = ToMilliseconds(scoring.LastSector1);
        this.Sector2Ms = ToMilliseconds(scoring.LastSector2 - scoring.LastSector1);
        this.Sector3Ms = ToMilliseconds(scoring.LastLapTime - scoring.LastSector2);
        this.TrackName = trackName;
        this.IsValid = scoring.CountLapFlag == 2;
    }

    public int CompletedLaps { get; }
    public string DriverName { get; }
    public bool IsValid { get; }
    public int LastLapTimeMs { get; }
    public Guid SessionId { get; }

    public int Sector1Ms { get; }
    public int Sector2Ms { get; }
    public int Sector3Ms { get; }
    public string TrackName { get; }
    public LmuVehicleClass VehicleClass { get; }
    public string VehicleClassName { get; }
    public string VehicleModel { get; }
    public string VehicleName { get; }

    private static int ToMilliseconds(double seconds)
    {
        return (int)Math.Round(seconds * 1000);
    }
}
