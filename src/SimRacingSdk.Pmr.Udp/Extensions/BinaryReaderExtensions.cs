using System.Text;
using SimRacingSdk.Pmr.Udp.Enums;
using SimRacingSdk.Pmr.Udp.Messages;

namespace SimRacingSdk.Pmr.Udp.Extensions;

internal static class BinaryReaderExtensions
{
    internal static PmrVector3 ReadPmrVector3(this BinaryReader reader)
    {
        return new PmrVector3
        {
            X = reader.ReadSingle(),
            Y = reader.ReadSingle(),
            Z = reader.ReadSingle()
        };
    }

    internal static PmrQuaternion ReadPmrQuaternion(this BinaryReader reader)
    {
        return new PmrQuaternion
        {
            X = reader.ReadSingle(),
            Y = reader.ReadSingle(),
            Z = reader.ReadSingle(),
            W = reader.ReadSingle()
        };
    }

    internal static string ReadPmrString(this BinaryReader reader)
    {
        var length = reader.ReadByte();
        var bytes = reader.ReadBytes(length);
        return Encoding.UTF8.GetString(bytes);
    }

    internal static IReadOnlyList<double> ReadPmrSecondsList(this BinaryReader reader)
    {
        var length = reader.ReadByte();
        var values = new double[length];
        for(var i = 0; i < length; i++)
        {
            values[i] = reader.ReadSingle();
        }

        return values;
    }

    internal static IReadOnlyList<float> ReadPmrFloatList(this BinaryReader reader)
    {
        var length = reader.ReadByte();
        var values = new float[length];
        for(var i = 0; i < length; i++)
        {
            values[i] = reader.ReadSingle();
        }

        return values;
    }

    internal static PmrRaceInfo ReadPmrRaceInfo(this BinaryReader reader)
    {
        return new PmrRaceInfo
        {
            Track = reader.ReadPmrString(),
            Layout = reader.ReadPmrString(),
            Season = reader.ReadPmrString(),
            Weather = reader.ReadPmrString(),
            Session = reader.ReadPmrString(),
            GameMode = reader.ReadPmrString(),
            LayoutLengthMeters = reader.ReadSingle(),
            DurationSeconds = reader.ReadSingle(),
            OvertimeSeconds = reader.ReadSingle(),
            AmbientTemperatureCelsius = reader.ReadSingle(),
            TrackTemperatureCelsius = reader.ReadSingle(),
            IsLaps = reader.ReadByte() != 0,
            State = (PmrRaceSessionState)reader.ReadByte(),
            NumberOfParticipants = reader.ReadByte()
        };
    }

    internal static PmrParticipantRaceState ReadPmrParticipantRaceState(this BinaryReader reader)
    {
        // The header (UDPTelemetryProtocol.h) and docs/packet_layout_example.txt say isPlayer comes
        // first, but the wire order is vehicleId then isPlayer, as the bundled SimpleUDPServer
        // sample reads it (and pairs telemetry to participants by vehicleId). Reading the header's
        // order shifts isPlayer into the vehicle id's top byte (a human read as 16777216), makes
        // the low byte of a real id look like an inverted isPlayer flag, and leaves participant ids
        // that never match telemetry ids.
        var vehicleId = reader.ReadInt32();
        var isPlayer = reader.ReadByte() != 0;
        var vehicleName = reader.ReadPmrString();
        var driverName = reader.ReadPmrString();
        var liveryId = reader.ReadPmrString();
        var vehicleClass = reader.ReadPmrString();
        var racePosition = reader.ReadInt32();
        var currentLap = reader.ReadInt32();
        var currentLapTimeSeconds = (double)reader.ReadSingle();
        var bestLapTimeSeconds = (double)reader.ReadSingle();
        var lapProgress = reader.ReadSingle();
        var currentSector = reader.ReadInt32();
        var currentSectorTimesSeconds = reader.ReadPmrSecondsList();
        var bestSectorTimesSeconds = reader.ReadPmrSecondsList();

        return new PmrParticipantRaceState
        {
            VehicleId = vehicleId,
            IsPlayer = isPlayer,
            VehicleName = vehicleName,
            DriverName = driverName,
            LiveryId = liveryId,
            VehicleClass = vehicleClass,
            RacePosition = racePosition,
            CurrentLap = currentLap,
            CurrentLapTimeSeconds = currentLapTimeSeconds,
            BestLapTimeSeconds = bestLapTimeSeconds,
            LapProgressFraction = lapProgress,
            CurrentSector = currentSector,
            CurrentSectorTimesSeconds = currentSectorTimesSeconds,
            BestSectorTimesSeconds = bestSectorTimesSeconds,
            InPits = reader.ReadByte() != 0,
            SessionFinished = reader.ReadByte() != 0,
            IsDisqualified = reader.ReadByte() != 0,
            Flags = (PmrRaceFlags)reader.ReadUInt32()
        };
    }

    internal static PmrVehicleTelemetryWheel ReadPmrVehicleTelemetryWheel(this BinaryReader reader)
    {
        return new PmrVehicleTelemetryWheel
        {
            ContactMaterialHash = reader.ReadInt32(),
            AngularVelocityRadiansPerSecond = reader.ReadSingle(),
            LinearSpeedMetersPerSecond = reader.ReadSingle(),
            SlideLocalSpaceMetersPerSecond = reader.ReadPmrVector3(),
            ForceLocalSpaceNewtons = reader.ReadPmrVector3(),
            MomentLocalSpaceNm = reader.ReadPmrVector3(),
            ContactRadiusMeters = reader.ReadSingle(),
            PressureKpa = reader.ReadSingle(),
            InclinationRadians = reader.ReadSingle(),
            SlipRatio = reader.ReadSingle(),
            SlipAngleRadians = reader.ReadSingle(),
            TreadTemperatureCelsius = reader.ReadPmrVector3(),
            CarcassTemperatureCelsius = reader.ReadSingle(),
            InternalAirTemperatureCelsius = reader.ReadSingle(),
            WellAirTemperatureCelsius = reader.ReadSingle(),
            RimTemperatureCelsius = reader.ReadSingle(),
            BrakeTemperatureCelsius = reader.ReadSingle(),
            SpringStrainFraction = reader.ReadSingle(),
            DamperVelocityMetersPerSecond = reader.ReadSingle(),
            HubTorqueNm = reader.ReadSingle(),
            HubPowerKw = reader.ReadSingle(),
            WheelTorqueNm = reader.ReadSingle(),
            WheelPowerKw = reader.ReadSingle()
        };
    }

    internal static PmrVehicleTelemetryChassis ReadPmrVehicleTelemetryChassis(this BinaryReader reader)
    {
        return new PmrVehicleTelemetryChassis
        {
            PositionWorldSpaceMeters = reader.ReadPmrVector3(),
            Orientation = reader.ReadPmrQuaternion(),
            AngularVelocityWorldSpaceRadiansPerSecond = reader.ReadPmrVector3(),
            AngularVelocityLocalSpaceRadiansPerSecond = reader.ReadPmrVector3(),
            VelocityWorldSpaceMetersPerSecond = reader.ReadPmrVector3(),
            VelocityLocalSpaceMetersPerSecond = reader.ReadPmrVector3(),
            AccelerationWorldSpaceMetersPerSecondSquared = reader.ReadPmrVector3(),
            AccelerationLocalSpaceMetersPerSecondSquared = reader.ReadPmrVector3(),
            OverallSpeedMetersPerSecond = reader.ReadSingle(),
            ForwardSpeedMetersPerSecond = reader.ReadSingle(),
            SideslipRadians = reader.ReadSingle()
        };
    }

    internal static PmrVehicleTelemetryGear ReadPmrVehicleTelemetryGear(this BinaryReader reader)
    {
        return new PmrVehicleTelemetryGear
        {
            UpshiftRpm = reader.ReadSingle(),
            DownshiftRpm = reader.ReadSingle()
        };
    }

    internal static PmrVehicleTelemetryDrivetrain ReadPmrVehicleTelemetryDrivetrain(this BinaryReader reader)
    {
        var engineRpm = reader.ReadSingle();
        var engineRevRatio = reader.ReadSingle();
        var engineTorqueNm = reader.ReadSingle();
        var enginePowerKw = reader.ReadSingle();
        var engineLoadFraction = reader.ReadSingle();
        var engineTurboRpm = reader.ReadSingle();
        var engineTurboBoostPressureKpa = reader.ReadSingle();
        var fuelRemainingLitres = reader.ReadSingle();
        var fuelUseRateLitresPerSecond = reader.ReadSingle();
        var engineOilPressureKpa = reader.ReadSingle();
        var engineOilTemperatureCelsius = reader.ReadSingle();
        var engineCoolantTemperatureCelsius = reader.ReadSingle();
        var exhaustGasTemperatureCelsius = reader.ReadSingle();
        var motorRpm = reader.ReadSingle();
        var batteryRemainingJoules = reader.ReadSingle();
        var batteryUseRateJoulesPerSecond = reader.ReadSingle();
        var transmissionRpm = reader.ReadSingle();
        var gearboxInputRpm = reader.ReadSingle();
        var gearboxOutputRpm = reader.ReadSingle();
        var gearboxTorqueNm = reader.ReadSingle();
        var gearboxPowerKw = reader.ReadSingle();
        var gearboxLoadInFraction = reader.ReadSingle();
        var gearboxLoadOutFraction = reader.ReadSingle();
        var timeSinceShiftSeconds = reader.ReadSingle();
        var estimatedDrivenSpeedMetersPerSecond = reader.ReadSingle();
        var outputTorqueNm = reader.ReadSingle();
        var outputPowerKw = reader.ReadSingle();
        var outputEfficiencyFraction = reader.ReadSingle();
        var starterActive = reader.ReadByte() != 0;
        var engineRunning = reader.ReadByte() != 0;
        var engineFanRunning = reader.ReadByte() != 0;
        var revLimiterActive = reader.ReadByte() != 0;
        var tractionControlActive = reader.ReadByte() != 0;
        var speedLimiterEnabled = reader.ReadByte() != 0;
        var speedLimiterActive = reader.ReadByte() != 0;

        var gearCount = reader.ReadByte();
        var gears = new PmrVehicleTelemetryGear[gearCount];
        for(var i = 0; i < gearCount; i++)
        {
            gears[i] = reader.ReadPmrVehicleTelemetryGear();
        }

        return new PmrVehicleTelemetryDrivetrain
        {
            EngineRpm = engineRpm,
            EngineRevRatio = engineRevRatio,
            EngineTorqueNm = engineTorqueNm,
            EnginePowerKw = enginePowerKw,
            EngineLoadFraction = engineLoadFraction,
            EngineTurboRpm = engineTurboRpm,
            EngineTurboBoostPressureKpa = engineTurboBoostPressureKpa,
            FuelRemainingLitres = fuelRemainingLitres,
            FuelUseRateLitresPerSecond = fuelUseRateLitresPerSecond,
            EngineOilPressureKpa = engineOilPressureKpa,
            EngineOilTemperatureCelsius = engineOilTemperatureCelsius,
            EngineCoolantTemperatureCelsius = engineCoolantTemperatureCelsius,
            ExhaustGasTemperatureCelsius = exhaustGasTemperatureCelsius,
            MotorRpm = motorRpm,
            BatteryRemainingJoules = batteryRemainingJoules,
            BatteryUseRateJoulesPerSecond = batteryUseRateJoulesPerSecond,
            TransmissionRpm = transmissionRpm,
            GearboxInputRpm = gearboxInputRpm,
            GearboxOutputRpm = gearboxOutputRpm,
            GearboxTorqueNm = gearboxTorqueNm,
            GearboxPowerKw = gearboxPowerKw,
            GearboxLoadInFraction = gearboxLoadInFraction,
            GearboxLoadOutFraction = gearboxLoadOutFraction,
            TimeSinceShiftSeconds = timeSinceShiftSeconds,
            EstimatedDrivenSpeedMetersPerSecond = estimatedDrivenSpeedMetersPerSecond,
            OutputTorqueNm = outputTorqueNm,
            OutputPowerKw = outputPowerKw,
            OutputEfficiencyFraction = outputEfficiencyFraction,
            StarterActive = starterActive,
            EngineRunning = engineRunning,
            EngineFanRunning = engineFanRunning,
            RevLimiterActive = revLimiterActive,
            TractionControlActive = tractionControlActive,
            SpeedLimiterEnabled = speedLimiterEnabled,
            SpeedLimiterActive = speedLimiterActive,
            Gears = gears
        };
    }

    internal static PmrVehicleTelemetrySuspension ReadPmrVehicleTelemetrySuspension(this BinaryReader reader)
    {
        var averageLoadsNewtons = reader.ReadPmrFloatList();
        return new PmrVehicleTelemetrySuspension
        {
            AverageLoadsNewtons = averageLoadsNewtons,
            LoadBiasFraction = reader.ReadSingle()
        };
    }

    internal static PmrVehicleTelemetryInput ReadPmrVehicleTelemetryInput(this BinaryReader reader)
    {
        return new PmrVehicleTelemetryInput
        {
            SteeringFraction = reader.ReadSingle(),
            AcceleratorFraction = reader.ReadSingle(),
            BrakeFraction = reader.ReadSingle(),
            ClutchFraction = reader.ReadSingle(),
            HandbrakeFraction = reader.ReadSingle(),
            Gear = reader.ReadInt32()
        };
    }

    internal static PmrVehicleTelemetrySetup ReadPmrVehicleTelemetrySetup(this BinaryReader reader)
    {
        return new PmrVehicleTelemetrySetup
        {
            BrakeBiasFraction = reader.ReadSingle(),
            FrontAntiRollStiffnessNmPerRadian = reader.ReadSingle(),
            RearAntiRollStiffnessNmPerRadian = reader.ReadSingle(),
            RegenLimitFraction = reader.ReadSingle(),
            DeployLimitFraction = reader.ReadSingle(),
            AbsLevel = reader.ReadByte(),
            TcsLevel = reader.ReadByte()
        };
    }

    internal static PmrVehicleTelemetryGeneral ReadPmrVehicleTelemetryGeneral(this BinaryReader reader)
    {
        return new PmrVehicleTelemetryGeneral
        {
            CenterOfGravityMeters = reader.ReadPmrVector3(),
            SteeringWheelAngleDegrees = reader.ReadSingle(),
            TotalMassKg = reader.ReadSingle(),
            DrivenWheelAngularVelocityRadiansPerSecond = reader.ReadSingle(),
            NonDrivenWheelAngularVelocityRadiansPerSecond = reader.ReadSingle(),
            EstimatedRollingSpeedMetersPerSecond = reader.ReadSingle(),
            EstimatedLinearSpeedMetersPerSecond = reader.ReadSingle(),
            TotalBrakeForceNewtons = reader.ReadSingle(),
            AbsActive = reader.ReadByte() != 0
        };
    }

    internal static PmrVehicleTelemetryConstant ReadPmrVehicleTelemetryConstant(this BinaryReader reader)
    {
        return new PmrVehicleTelemetryConstant
        {
            ChassisBoundingBoxMinMeters = reader.ReadPmrVector3(),
            ChassisBoundingBoxMaxMeters = reader.ReadPmrVector3(),
            StarterIdleRpm = reader.ReadSingle(),
            EngineTorquePeakRpm = reader.ReadSingle(),
            EnginePowerPeakRpm = reader.ReadSingle(),
            EngineMaxRpm = reader.ReadSingle(),
            EngineMaxTorqueNm = reader.ReadSingle(),
            EngineMaxPowerKw = reader.ReadSingle(),
            EngineMaxBoostKpa = reader.ReadSingle(),
            FuelCapacityLitres = reader.ReadSingle(),
            BatteryCapacityJoules = reader.ReadSingle(),
            TrackWidthFrontMeters = reader.ReadSingle(),
            TrackWidthRearMeters = reader.ReadSingle(),
            WheelbaseMeters = reader.ReadSingle(),
            NumberOfWheels = reader.ReadByte(),
            NumberOfForwardGears = reader.ReadByte(),
            NumberOfReverseGears = reader.ReadByte(),
            IsHybrid = reader.ReadByte() != 0
        };
    }

    internal static PmrVehicleTelemetry ReadPmrVehicleTelemetry(this BinaryReader reader)
    {
        var vehicleId = reader.ReadInt32();

        var wheelCount = reader.ReadByte();
        var wheels = new PmrVehicleTelemetryWheel[wheelCount];
        for(var i = 0; i < wheelCount; i++)
        {
            wheels[i] = reader.ReadPmrVehicleTelemetryWheel();
        }

        return new PmrVehicleTelemetry
        {
            VehicleId = vehicleId,
            Wheels = wheels,
            Chassis = reader.ReadPmrVehicleTelemetryChassis(),
            Drivetrain = reader.ReadPmrVehicleTelemetryDrivetrain(),
            Suspension = reader.ReadPmrVehicleTelemetrySuspension(),
            Input = reader.ReadPmrVehicleTelemetryInput(),
            Setup = reader.ReadPmrVehicleTelemetrySetup(),
            General = reader.ReadPmrVehicleTelemetryGeneral(),
            Constant = reader.ReadPmrVehicleTelemetryConstant()
        };
    }
}
