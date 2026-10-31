using SimRacingSdk.R3e.SharedMemory.Enums;
using SimRacingSdk.R3e.SharedMemory.Messages;
using SimRacingSdk.R3e.SharedMemory.Services;

namespace SimRacingSdk.R3e.SharedMemory.Models;

public record R3eSharedMemoryLap
{
    // R3E's sector times are cumulative: sector 3 is the lap time.
    internal R3eSharedMemoryLap(R3eDriverData driver, R3eSectors? previousLapSectors, int lapValidity,
        Guid sessionId, bool isLocalPlayer)
    {
        var driverInfo = driver.DriverInfo;
        this.SessionId = sessionId;
        this.SlotId = driverInfo.SlotId;
        this.UserId = R3eValues.Available(driverInfo.UserId);
        this.DriverName = R3eValues.DecodeName(driverInfo.Name);
        this.CarNumber = driverInfo.CarNumber;
        this.ModelId = driverInfo.ModelId;
        this.ClassId = driverInfo.ClassId;
        this.ManufacturerId = driverInfo.ManufacturerId;
        this.TeamId = driverInfo.TeamId;
        this.LiveryId = driverInfo.LiveryId;
        this.IsLocalPlayer = isLocalPlayer;
        this.CompletedLaps = driver.CompletedLaps;
        this.Place = driver.Place;
        this.PlaceClass = driver.PlaceClass;
        this.FinishStatus = (R3eFinishStatus)driver.FinishStatus;
        this.IsValid = R3eValues.AvailableFlag(lapValidity);

        if(previousLapSectors is { } sectors)
        {
            this.LapTimeMs = R3eValues.ToMilliseconds(sectors.Sector3);
            this.Sector1Ms = R3eValues.ToMilliseconds(sectors.Sector1);
            this.Sector2Ms = R3eValues.ToMilliseconds(sectors.Sector2 - sectors.Sector1);
            this.Sector3Ms = R3eValues.ToMilliseconds(sectors.Sector3 - sectors.Sector2);
        }
    }

    public int CarNumber { get; }
    public int ClassId { get; }
    public int CompletedLaps { get; }
    public string DriverName { get; }
    public R3eFinishStatus FinishStatus { get; }
    public bool IsLocalPlayer { get; }
    public bool? IsValid { get; }
    public int? LapTimeMs { get; }
    public int LiveryId { get; }
    public int ManufacturerId { get; }
    public int ModelId { get; }
    public int Place { get; }
    public int PlaceClass { get; }
    public int? Sector1Ms { get; }
    public int? Sector2Ms { get; }
    public int? Sector3Ms { get; }
    public Guid SessionId { get; }
    public int SlotId { get; }
    public int TeamId { get; }
    public int? UserId { get; }
}
