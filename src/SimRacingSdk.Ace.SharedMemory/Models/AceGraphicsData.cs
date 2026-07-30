#nullable disable

using SimRacingSdk.Ace.SharedMemory.Enums;
using SimRacingSdk.Ace.SharedMemory.Messages;

namespace SimRacingSdk.Ace.SharedMemory.Models;

public record AceGraphicsData
{
    private readonly AceGraphicsPage graphicsPage;

    internal AceGraphicsData()
    {
        this.IsEmpty = true;
    }

    // Instrumentation/Electronics min/max/is-modifiable capability descriptors from the PDF are
    // deliberately not surfaced here - they describe setup limits, not live telemetry.
    internal AceGraphicsData(AceGraphicsPage graphicsPage)
    {
        this.graphicsPage = graphicsPage;
        this.PacketId = graphicsPage.PacketId;
        this.Status = graphicsPage.Status;
        this.FocusedCarId = new AceCarIdPair { A = graphicsPage.FocusedCarIdA, B = graphicsPage.FocusedCarIdB };
        this.PlayerCarId = new AceCarIdPair { A = graphicsPage.PlayerCarIdA, B = graphicsPage.PlayerCarIdB };
        this.Rpm = graphicsPage.Rpm;
        this.IsRpmLimiterOn = graphicsPage.IsRpmLimiterOn;
        this.TcActive = graphicsPage.TcActive;
        this.AbsActive = graphicsPage.AbsActive;
        this.EscActive = graphicsPage.EscActive;
        this.LaunchActive = graphicsPage.LaunchActive;
        this.IsIgnitionOn = graphicsPage.IsIgnitionOn;
        this.IsEngineRunning = graphicsPage.IsEngineRunning;
        this.IsWrongWay = graphicsPage.IsWrongWay;
        this.IsDrsAvailable = graphicsPage.IsDrsAvailable;
        this.DisplaySpeedKmh = graphicsPage.DisplaySpeedKmh;
        this.DisplaySpeedMph = graphicsPage.DisplaySpeedMph;
        this.GearInt = graphicsPage.GearInt;
        this.RpmPercent = graphicsPage.RpmPercent;
        this.GasPercent = graphicsPage.GasPercent;
        this.BrakePercent = graphicsPage.BrakePercent;
        this.ClutchPercent = graphicsPage.ClutchPercent;
        this.SteeringPercent = graphicsPage.SteeringPercent;
        this.WaterTemperatureC = graphicsPage.WaterTemperatureC;
        this.AirTemperatureC = graphicsPage.AirTemperatureC;
        this.OilTemperatureC = graphicsPage.OilTemperatureC;
        this.TurboBoost = graphicsPage.TurboBoost;
        this.CurrentKm = graphicsPage.CurrentKm;
        this.TotalKm = graphicsPage.TotalKm;
        this.DeltaTimeMs = graphicsPage.DeltaTimeMs;
        this.CurrentLapTimeMs = graphicsPage.CurrentLapTimeMs;
        this.PredictedLapTimeMs = graphicsPage.PredictedLapTimeMs;
        this.FuelLiterCurrentQuantity = graphicsPage.FuelLiterCurrentQuantity;
        this.FuelLiterCurrentQuantityPercent = graphicsPage.FuelLiterCurrentQuantityPercent;
        this.CurrentTorque = graphicsPage.CurrentTorque;
        this.CurrentBhp = graphicsPage.CurrentBhp;
        this.TyreLf = graphicsPage.TyreLf;
        this.TyreRf = graphicsPage.TyreRf;
        this.TyreLr = graphicsPage.TyreLr;
        this.TyreRr = graphicsPage.TyreRr;
        this.NormalizedPosition = graphicsPage.NormalizedPosition;
        this.CarDamage = graphicsPage.CarDamage;
        this.CarLocation = graphicsPage.CarLocation;
        this.PitInfo = graphicsPage.PitInfo;
        this.FuelLiterUsed = graphicsPage.FuelLiterUsed;
        this.FuelLiterPerLap = graphicsPage.FuelLiterPerLap;
        this.LapsPossibleWithFuel = graphicsPage.LapsPossibleWithFuel;
        this.Instrumentation = graphicsPage.Instrumentation;
        this.Electronics = graphicsPage.Electronics;
        this.TotalLapCount = graphicsPage.TotalLapCount;
        this.CurrentPosition = graphicsPage.CurrentPosition;
        this.TotalDrivers = graphicsPage.TotalDrivers;
        this.LastLapTimeMs = graphicsPage.LastLapTimeMs;
        this.BestLapTimeMs = graphicsPage.BestLapTimeMs;
        this.Flag = graphicsPage.Flag;
        this.GlobalFlag = graphicsPage.GlobalFlag;
        this.MaxGears = graphicsPage.MaxGears;
        this.EngineType = graphicsPage.EngineType;
        this.HasKers = graphicsPage.HasKers;
        this.IsLastLap = graphicsPage.IsLastLap;
        this.PerformanceModeName = graphicsPage.PerformanceModeName;
        this.SessionState = graphicsPage.SessionState;
        this.TimingState = graphicsPage.TimingState;
        this.DriverName = graphicsPage.DriverName;
        this.DriverSurname = graphicsPage.DriverSurname;
        this.CarModel = graphicsPage.CarModel;
        this.IsInPitBox = graphicsPage.IsInPitBox;
        this.IsInPitLane = graphicsPage.IsInPitLane;
        this.IsValidLap = graphicsPage.IsValidLap;
        this.CarCoordinates = graphicsPage.CarCoordinates;
        this.CarIds = graphicsPage.CarIds;
        this.GapAhead = graphicsPage.GapAhead;
        this.GapBehind = graphicsPage.GapBehind;
        this.ActiveCars = graphicsPage.ActiveCars;
        this.AssistsState = graphicsPage.AssistsState;
        this.MaxFuel = graphicsPage.MaxFuel;
        this.MaxTurboBoost = graphicsPage.MaxTurboBoost;
        this.UseSingleCompound = graphicsPage.UseSingleCompound;
    }

    public int PacketId { get; }
    public AceStatus Status { get; }
    public AceCarIdPair FocusedCarId { get; }
    public AceCarIdPair PlayerCarId { get; }
    public ushort Rpm { get; }
    public bool IsRpmLimiterOn { get; }
    public bool TcActive { get; }
    public bool AbsActive { get; }
    public bool EscActive { get; }
    public bool LaunchActive { get; }
    public bool IsIgnitionOn { get; }
    public bool IsEngineRunning { get; }
    public bool IsWrongWay { get; }
    public bool IsDrsAvailable { get; }
    public short DisplaySpeedKmh { get; }
    public short DisplaySpeedMph { get; }
    public short GearInt { get; }
    public float RpmPercent { get; }
    public float GasPercent { get; }
    public float BrakePercent { get; }
    public float ClutchPercent { get; }
    public float SteeringPercent { get; }
    public sbyte WaterTemperatureC { get; }
    public sbyte AirTemperatureC { get; }
    public float OilTemperatureC { get; }
    public float TurboBoost { get; }
    public float CurrentKm { get; }
    public uint TotalKm { get; }
    public int DeltaTimeMs { get; }
    public int CurrentLapTimeMs { get; }
    public int PredictedLapTimeMs { get; }
    public float FuelLiterCurrentQuantity { get; }
    public float FuelLiterCurrentQuantityPercent { get; }
    public float CurrentTorque { get; }
    public int CurrentBhp { get; }
    public AceTyreState TyreLf { get; }
    public AceTyreState TyreRf { get; }
    public AceTyreState TyreLr { get; }
    public AceTyreState TyreRr { get; }
    public float NormalizedPosition { get; }
    public AceDamageState CarDamage { get; }
    public AceCarLocation CarLocation { get; }
    public AcePitInfo PitInfo { get; }
    public float FuelLiterUsed { get; }
    public float FuelLiterPerLap { get; }
    public float LapsPossibleWithFuel { get; }
    public AceInstrumentation Instrumentation { get; }
    public AceElectronics Electronics { get; }
    public bool IsEmpty { get; }
    public int TotalLapCount { get; }
    public uint CurrentPosition { get; }
    public uint TotalDrivers { get; }
    public int LastLapTimeMs { get; }
    public int BestLapTimeMs { get; }
    public AceFlagType Flag { get; }
    public AceFlagType GlobalFlag { get; }
    public uint MaxGears { get; }
    public AceEngineType EngineType { get; }
    public bool HasKers { get; }
    public bool IsLastLap { get; }
    public string PerformanceModeName { get; }
    public AceSessionState SessionState { get; }
    public AceTimingState TimingState { get; }
    public string DriverName { get; }
    public string DriverSurname { get; }
    public string CarModel { get; }
    public bool IsInPitBox { get; }
    public bool IsInPitLane { get; }
    public bool IsValidLap { get; }
    public AceCoordinate3d[] CarCoordinates { get; }
    public AceCarIdPair[] CarIds { get; }
    public float GapAhead { get; }
    public float GapBehind { get; }
    public byte ActiveCars { get; }
    public AceAssistsState AssistsState { get; }
    public float MaxFuel { get; }
    public float MaxTurboBoost { get; }
    public bool UseSingleCompound { get; }
    public DateTime TimeStamp { get; } = DateTime.UtcNow;

    // Array-typed members (CarCoordinates/CarIds) need explicit formatting here - the compiler's
    // generated PrintMembers would otherwise print Array.ToString()'s type name instead of the
    // per-car values these exist to expose.
    protected virtual bool PrintMembers(System.Text.StringBuilder builder)
    {
        builder.Append($"PacketId = {this.PacketId}, Status = {this.Status}, FocusedCarId = {this.FocusedCarId}, "
            + $"PlayerCarId = {this.PlayerCarId}, Rpm = {this.Rpm}, IsRpmLimiterOn = {this.IsRpmLimiterOn}, "
            + $"TcActive = {this.TcActive}, AbsActive = {this.AbsActive}, EscActive = {this.EscActive}, "
            + $"LaunchActive = {this.LaunchActive}, IsIgnitionOn = {this.IsIgnitionOn}, "
            + $"IsEngineRunning = {this.IsEngineRunning}, IsWrongWay = {this.IsWrongWay}, "
            + $"IsDrsAvailable = {this.IsDrsAvailable}, DisplaySpeedKmh = {this.DisplaySpeedKmh}, "
            + $"DisplaySpeedMph = {this.DisplaySpeedMph}, GearInt = {this.GearInt}, RpmPercent = {this.RpmPercent}, "
            + $"GasPercent = {this.GasPercent}, BrakePercent = {this.BrakePercent}, ClutchPercent = {this.ClutchPercent}, "
            + $"SteeringPercent = {this.SteeringPercent}, WaterTemperatureC = {this.WaterTemperatureC}, "
            + $"AirTemperatureC = {this.AirTemperatureC}, OilTemperatureC = {this.OilTemperatureC}, "
            + $"TurboBoost = {this.TurboBoost}, CurrentKm = {this.CurrentKm}, TotalKm = {this.TotalKm}, "
            + $"DeltaTimeMs = {this.DeltaTimeMs}, CurrentLapTimeMs = {this.CurrentLapTimeMs}, "
            + $"PredictedLapTimeMs = {this.PredictedLapTimeMs}, "
            + $"FuelLiterCurrentQuantity = {this.FuelLiterCurrentQuantity}, "
            + $"FuelLiterCurrentQuantityPercent = {this.FuelLiterCurrentQuantityPercent}, "
            + $"CurrentTorque = {this.CurrentTorque}, CurrentBhp = {this.CurrentBhp}, TyreLf = {this.TyreLf}, "
            + $"TyreRf = {this.TyreRf}, TyreLr = {this.TyreLr}, TyreRr = {this.TyreRr}, "
            + $"NormalizedPosition = {this.NormalizedPosition}, CarDamage = {this.CarDamage}, "
            + $"CarLocation = {this.CarLocation}, PitInfo = {this.PitInfo}, FuelLiterUsed = {this.FuelLiterUsed}, "
            + $"FuelLiterPerLap = {this.FuelLiterPerLap}, LapsPossibleWithFuel = {this.LapsPossibleWithFuel}, "
            + $"Instrumentation = {this.Instrumentation}, Electronics = {this.Electronics}, IsEmpty = {this.IsEmpty}, "
            + $"TotalLapCount = {this.TotalLapCount}, CurrentPosition = {this.CurrentPosition}, "
            + $"TotalDrivers = {this.TotalDrivers}, LastLapTimeMs = {this.LastLapTimeMs}, "
            + $"BestLapTimeMs = {this.BestLapTimeMs}, Flag = {this.Flag}, GlobalFlag = {this.GlobalFlag}, "
            + $"MaxGears = {this.MaxGears}, EngineType = {this.EngineType}, HasKers = {this.HasKers}, "
            + $"IsLastLap = {this.IsLastLap}, PerformanceModeName = {this.PerformanceModeName}, "
            + $"SessionState = {this.SessionState}, TimingState = {this.TimingState}, DriverName = {this.DriverName}, "
            + $"DriverSurname = {this.DriverSurname}, CarModel = {this.CarModel}, IsInPitBox = {this.IsInPitBox}, "
            + $"IsInPitLane = {this.IsInPitLane}, IsValidLap = {this.IsValidLap}, "
            + $"CarCoordinates = {SharedMemoryLogFormatting.FormatArray(this.CarCoordinates)}, "
            + $"CarIds = {SharedMemoryLogFormatting.FormatArray(this.CarIds)}, GapAhead = {this.GapAhead}, "
            + $"GapBehind = {this.GapBehind}, ActiveCars = {this.ActiveCars}, AssistsState = {this.AssistsState}, "
            + $"MaxFuel = {this.MaxFuel}, MaxTurboBoost = {this.MaxTurboBoost}, "
            + $"UseSingleCompound = {this.UseSingleCompound}, TimeStamp = {this.TimeStamp}");
        return true;
    }
}
