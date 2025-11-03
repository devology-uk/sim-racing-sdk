#nullable disable

namespace SimRacingSdk.Ams2.Core.Models;

public record Ams2CarInfo
{
    private string ams2CarId;

    public string Ams2CarId
    {
        get => this.ams2CarId ?? this.Model.Replace(" ", "_");
        set => this.ams2CarId = value;
    }

    public string Class { get; set; }
    public string Differential { get; set; }
    public double DisplacementL { get; set; }
    public string Driveline { get; set; }
    public string Electronics { get; set; }
    public string Engine { get; set; }
    public string EnginePlacement { get; set; }
    public bool HasAdjustableTurbo { get; set; }
    public bool HasBoostButton { get; set; }
    public bool HasDrs { get; set; }
    public bool HasHeadlights { get; set; }
    public bool HasOnBoardBrakeBias { get; set; }
    public bool HasOnBoardRollBars { get; set; }
    public bool HasPitSpeedLimiter { get; set; }
    public string Manufacturer { get; set; }
    public string Model { get; set; }
    public double PowerHp { get; set; }
    public int TopSpeedKph { get; set; }
    public double TorqueNm { get; set; }
    public string Transmission { get; set; }
    public string WeightDistribution { get; set; }
    public double WeightKg { get; set; }
    public double WheelBaseM { get; set; }
}