#nullable disable

using SimRacingSdk.Core;
using SimRacingSdk.Lmu.Core.Models;

namespace SimRacingSdk.Lmu.Demo.ResultExplorer;

public record DriverItem
{
    public DriverItem(LmuDriver lmuDriver)
    {
        this.BestLap = lmuDriver.BestLap.ToTimingStringFromSeconds();
        this.Car = lmuDriver.VehicleName;
        this.CarClass = lmuDriver.CarClass;
        this.CarType = lmuDriver.CarType;
        this.Category = lmuDriver.Category;
        this.ClassGridPosition = lmuDriver.ClassGridPosition;
        this.ClassPosition = lmuDriver.ClassPosition;
        this.FinishStatus = lmuDriver.FinishStatus;
        this.FinishTime = lmuDriver.FinishTime.ToTimingStringFromSeconds();
        this.GridPosition = lmuDriver.GridPosition;
        this.IsPlayer = lmuDriver.IsPlayer != 0;
        this.LapsCompleted = lmuDriver.LapCount;
        this.Name = lmuDriver.Name;
        this.PitStops = lmuDriver.PitStops;
        this.Position = lmuDriver.Position;
        this.RaceNumber = lmuDriver.CarNumber;
        this.TeamName = lmuDriver.TeamName;
    }

    public string BestLap { get; set; }
    public string Car { get; set; }
    public string CarClass { get; set; }
    public string CarType { get; set; }
    public string Category { get; set; }
    public int ClassGridPosition { get; set; }
    public int ClassPosition { get; set; }
    public string FinishStatus { get; set; }
    public string FinishTime { get; set; }
    public int GridPosition { get; set; }
    public bool IsPlayer { get; set; }
    public int LapsCompleted { get; set; }
    public string Name { get; set; }
    public int PitStops { get; set; }
    public int Position { get; set; }
    public int RaceNumber { get; set; }
    public string TeamName { get; set; }
}