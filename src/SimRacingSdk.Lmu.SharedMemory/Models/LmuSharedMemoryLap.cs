using SimRacingSdk.Lmu.SharedMemory.Enums;
using SimRacingSdk.Lmu.SharedMemory.Messages;

namespace SimRacingSdk.Lmu.SharedMemory.Models;

public record LmuSharedMemoryLap
{
    // VehicleModel (from Telemetry) is the catalog-matchable car identity (e.g. "911GT3R_2024"-style display
    // name); Scoring's VehicleName is the driver's entry/livery string (team name + car number), not a car model -
    // confirmed against a real rig log, see CLAUDE.md. VehicleClassName (Scoring.VehicleClass, a string) and
    // VehicleClass (Telemetry's own enum) come from different structs despite the name clash.
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
    }

    public int CompletedLaps { get; }
    public string DriverName { get; }
    public int LastLapTimeMs { get; }
    public Guid SessionId { get; }

    // LastSector1/LastSector2 are cumulative-from-lap-start, matching the sector convention already used elsewhere
    // in this SDK (Acc/Ams2) - confirmed against a real rig log, see CLAUDE.md.
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
