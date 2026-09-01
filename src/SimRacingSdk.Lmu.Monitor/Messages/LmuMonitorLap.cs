#nullable disable

using SimRacingSdk.Lmu.Monitor.Abstractions;

namespace SimRacingSdk.Lmu.Monitor.Messages;

public record LmuMonitorLap : LmuMonitorMessageBase
{
    public string CarManufacturer { get; init; }
    public int CompletedLaps { get; init; }
    public string DriverName { get; init; }
    public int LastLapTimeMs { get; init; }
    public Guid SessionId { get; init; }
    public int Sector1Ms { get; init; }
    public int Sector2Ms { get; init; }
    public int Sector3Ms { get; init; }
    public string TrackName { get; init; }
    public string VehicleName { get; init; }
}
