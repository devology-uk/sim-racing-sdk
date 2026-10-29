using SimRacingSdk.R3e.SharedMemory.Enums;
using SimRacingSdk.R3e.SharedMemory.Messages;
using SimRacingSdk.R3e.SharedMemory.Services;

namespace SimRacingSdk.R3e.SharedMemory.Models;

public record R3eWheelData
{
    internal R3eWheelData(R3eShared data, R3eCorner corner)
    {
        var tyreTemperature = data.TireTemp.For(corner);
        var brakeTemperature = data.BrakeTemp.For(corner);
        var player = data.Player;

        this.Corner = corner;
        this.TyrePressureKpa = R3eValues.Available(data.TirePressure.For(corner));
        this.TyreTempLeftCelsius = R3eValues.Available(tyreTemperature.CurrentTemp.Left);
        this.TyreTempCenterCelsius = R3eValues.Available(tyreTemperature.CurrentTemp.Center);
        this.TyreTempRightCelsius = R3eValues.Available(tyreTemperature.CurrentTemp.Right);
        this.TyreOptimalTempCelsius = R3eValues.Available(tyreTemperature.OptimalTemp);
        this.TyreColdTempCelsius = R3eValues.Available(tyreTemperature.ColdTemp);
        this.TyreHotTempCelsius = R3eValues.Available(tyreTemperature.HotTemp);
        this.TyreWear = R3eValues.Available(data.TireWear.For(corner));
        this.TyreGrip = R3eValues.Available(data.TireGrip.For(corner));
        this.TyreDirt = R3eValues.Available(data.TireDirt.For(corner));
        this.TyreLoadNewtons = R3eValues.Available(data.TireLoad.For(corner));
        this.TyreRadiansPerSecond = data.TireRps.For(corner);
        this.TyreSpeedMetersPerSecond = data.TireSpeed.For(corner);
        this.HasFlatSpot = R3eValues.AvailableFlag(data.TireFlatspot.For(corner));
        this.SurfaceMaterial = data.TireOnMtrl.For(corner);
        this.BrakeTempCelsius = R3eValues.Available(brakeTemperature.CurrentTemp);
        this.BrakeOptimalTempCelsius = R3eValues.Available(brakeTemperature.OptimalTemp);
        this.BrakePressureKilonewtons = R3eValues.Available(data.BrakePressure.For(corner));
        this.SuspensionDeflectionMeters = player.SuspensionDeflection.For(corner);
        this.SuspensionVelocityMetersPerSecond = player.SuspensionVelocity.For(corner);
        this.CamberRadians = player.Camber.For(corner);
        this.RideHeightMeters = player.RideHeight.For(corner);
    }

    public float? BrakeOptimalTempCelsius { get; }
    public float? BrakePressureKilonewtons { get; }
    public float? BrakeTempCelsius { get; }
    public double CamberRadians { get; }
    public R3eCorner Corner { get; }
    public bool? HasFlatSpot { get; }
    public double RideHeightMeters { get; }
    public int SurfaceMaterial { get; }
    public double SuspensionDeflectionMeters { get; }
    public double SuspensionVelocityMetersPerSecond { get; }
    public float? TyreColdTempCelsius { get; }
    public float? TyreDirt { get; }
    public float? TyreGrip { get; }
    public float? TyreHotTempCelsius { get; }
    public float? TyreLoadNewtons { get; }
    public float? TyreOptimalTempCelsius { get; }
    public float? TyrePressureKpa { get; }
    public float TyreRadiansPerSecond { get; }
    public float TyreSpeedMetersPerSecond { get; }
    public float? TyreTempCenterCelsius { get; }
    public float? TyreTempLeftCelsius { get; }
    public float? TyreTempRightCelsius { get; }
    public float? TyreWear { get; }
}
