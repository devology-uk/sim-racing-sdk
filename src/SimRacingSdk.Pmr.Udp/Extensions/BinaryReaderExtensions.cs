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

    internal static IReadOnlyList<TimeSpan> ReadPmrTimeSpanList(this BinaryReader reader)
    {
        var length = reader.ReadByte();
        var values = new TimeSpan[length];
        for(var i = 0; i < length; i++)
        {
            values[i] = TimeSpan.FromSeconds(reader.ReadSingle());
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
            LayoutLength = reader.ReadSingle(),
            Duration = reader.ReadSingle(),
            Overtime = reader.ReadSingle(),
            AmbientTemperature = reader.ReadSingle(),
            TrackTemperature = reader.ReadSingle(),
            IsLaps = reader.ReadByte() != 0,
            State = (PmrRaceSessionState)reader.ReadByte(),
            NumberOfParticipants = reader.ReadByte()
        };
    }

    internal static PmrParticipantRaceState ReadPmrParticipantRaceState(this BinaryReader reader)
    {
        // Header (UDPTelemetryProtocol.h) and docs/packet_layout_example.txt both order
        // isPlayer before vehicleId; the bundled SimpleUDPServer sample reads them the other
        // way round, which does not match either documented source.
        var isPlayer = reader.ReadByte() != 0;
        var vehicleId = reader.ReadInt32();
        var vehicleName = reader.ReadPmrString();
        var driverName = reader.ReadPmrString();
        var liveryId = reader.ReadPmrString();
        var vehicleClass = reader.ReadPmrString();
        var racePosition = reader.ReadInt32();
        var currentLap = reader.ReadInt32();
        var currentLapTime = TimeSpan.FromSeconds(reader.ReadSingle());
        var bestLapTime = TimeSpan.FromSeconds(reader.ReadSingle());
        var lapProgress = reader.ReadSingle();
        var currentSector = reader.ReadInt32();
        var currentSectorTimes = reader.ReadPmrTimeSpanList();
        var bestSectorTimes = reader.ReadPmrTimeSpanList();

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
            CurrentLapTime = currentLapTime,
            BestLapTime = bestLapTime,
            LapProgress = lapProgress,
            CurrentSector = currentSector,
            CurrentSectorTimes = currentSectorTimes,
            BestSectorTimes = bestSectorTimes,
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
            AngularVelocity = reader.ReadSingle(),
            LinearSpeed = reader.ReadSingle(),
            SlideLocalSpace = reader.ReadPmrVector3(),
            ForceLocalSpace = reader.ReadPmrVector3(),
            MomentLocalSpace = reader.ReadPmrVector3(),
            ContactRadius = reader.ReadSingle(),
            Pressure = reader.ReadSingle(),
            Inclination = reader.ReadSingle(),
            SlipRatio = reader.ReadSingle(),
            SlipAngle = reader.ReadSingle(),
            Tread = reader.ReadPmrVector3(),
            Carcass = reader.ReadSingle(),
            InternalAir = reader.ReadSingle(),
            WellAir = reader.ReadSingle(),
            Rim = reader.ReadSingle(),
            Brake = reader.ReadSingle(),
            SpringStrain = reader.ReadSingle(),
            DamperVelocity = reader.ReadSingle(),
            HubTorque = reader.ReadSingle(),
            HubPower = reader.ReadSingle(),
            WheelTorque = reader.ReadSingle(),
            WheelPower = reader.ReadSingle()
        };
    }

    internal static PmrVehicleTelemetryChassis ReadPmrVehicleTelemetryChassis(this BinaryReader reader)
    {
        return new PmrVehicleTelemetryChassis
        {
            PositionWorldSpace = reader.ReadPmrVector3(),
            Orientation = reader.ReadPmrQuaternion(),
            AngularVelocityWorldSpace = reader.ReadPmrVector3(),
            AngularVelocityLocalSpace = reader.ReadPmrVector3(),
            VelocityWorldSpace = reader.ReadPmrVector3(),
            VelocityLocalSpace = reader.ReadPmrVector3(),
            AccelerationWorldSpace = reader.ReadPmrVector3(),
            AccelerationLocalSpace = reader.ReadPmrVector3(),
            OverallSpeed = reader.ReadSingle(),
            ForwardSpeed = reader.ReadSingle(),
            Sideslip = reader.ReadSingle()
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
        var engineTorque = reader.ReadSingle();
        var enginePower = reader.ReadSingle();
        var engineLoad = reader.ReadSingle();
        var engineTurboRpm = reader.ReadSingle();
        var engineTurboBoostPressure = reader.ReadSingle();
        var fuelRemaining = reader.ReadSingle();
        var fuelUseRate = reader.ReadSingle();
        var engineOilPressure = reader.ReadSingle();
        var engineOilTemperature = reader.ReadSingle();
        var engineCoolantTemperature = reader.ReadSingle();
        var exhaustGasTemperature = reader.ReadSingle();
        var motorRpm = reader.ReadSingle();
        var batteryRemaining = reader.ReadSingle();
        var batteryUseRate = reader.ReadSingle();
        var transmissionRpm = reader.ReadSingle();
        var gearboxInputRpm = reader.ReadSingle();
        var gearboxOutputRpm = reader.ReadSingle();
        var gearboxTorque = reader.ReadSingle();
        var gearboxPower = reader.ReadSingle();
        var gearboxLoadIn = reader.ReadSingle();
        var gearboxLoadOut = reader.ReadSingle();
        var timeSinceShift = reader.ReadSingle();
        var estimatedDrivenSpeed = reader.ReadSingle();
        var outputTorque = reader.ReadSingle();
        var outputPower = reader.ReadSingle();
        var outputEfficiency = reader.ReadSingle();
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
            EngineTorque = engineTorque,
            EnginePower = enginePower,
            EngineLoad = engineLoad,
            EngineTurboRpm = engineTurboRpm,
            EngineTurboBoostPressure = engineTurboBoostPressure,
            FuelRemaining = fuelRemaining,
            FuelUseRate = fuelUseRate,
            EngineOilPressure = engineOilPressure,
            EngineOilTemperature = engineOilTemperature,
            EngineCoolantTemperature = engineCoolantTemperature,
            ExhaustGasTemperature = exhaustGasTemperature,
            MotorRpm = motorRpm,
            BatteryRemaining = batteryRemaining,
            BatteryUseRate = batteryUseRate,
            TransmissionRpm = transmissionRpm,
            GearboxInputRpm = gearboxInputRpm,
            GearboxOutputRpm = gearboxOutputRpm,
            GearboxTorque = gearboxTorque,
            GearboxPower = gearboxPower,
            GearboxLoadIn = gearboxLoadIn,
            GearboxLoadOut = gearboxLoadOut,
            TimeSinceShift = timeSinceShift,
            EstimatedDrivenSpeed = estimatedDrivenSpeed,
            OutputTorque = outputTorque,
            OutputPower = outputPower,
            OutputEfficiency = outputEfficiency,
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
        var averageLoads = reader.ReadPmrFloatList();
        return new PmrVehicleTelemetrySuspension
        {
            AverageLoads = averageLoads,
            LoadBias = reader.ReadSingle()
        };
    }

    internal static PmrVehicleTelemetryInput ReadPmrVehicleTelemetryInput(this BinaryReader reader)
    {
        return new PmrVehicleTelemetryInput
        {
            Steering = reader.ReadSingle(),
            Accelerator = reader.ReadSingle(),
            Brake = reader.ReadSingle(),
            Clutch = reader.ReadSingle(),
            Handbrake = reader.ReadSingle(),
            Gear = reader.ReadInt32()
        };
    }

    internal static PmrVehicleTelemetrySetup ReadPmrVehicleTelemetrySetup(this BinaryReader reader)
    {
        return new PmrVehicleTelemetrySetup
        {
            BrakeBias = reader.ReadSingle(),
            FrontAntiRollStiffness = reader.ReadSingle(),
            RearAntiRollStiffness = reader.ReadSingle(),
            RegenLimit = reader.ReadSingle(),
            DeployLimit = reader.ReadSingle(),
            AbsLevel = reader.ReadByte(),
            TcsLevel = reader.ReadByte()
        };
    }

    internal static PmrVehicleTelemetryGeneral ReadPmrVehicleTelemetryGeneral(this BinaryReader reader)
    {
        return new PmrVehicleTelemetryGeneral
        {
            CenterOfGravity = reader.ReadPmrVector3(),
            SteeringWheelAngle = reader.ReadSingle(),
            TotalMass = reader.ReadSingle(),
            DrivenWheelAngularVelocity = reader.ReadSingle(),
            NonDrivenWheelAngularVelocity = reader.ReadSingle(),
            EstimatedRollingSpeed = reader.ReadSingle(),
            EstimatedLinearSpeed = reader.ReadSingle(),
            TotalBrakeForce = reader.ReadSingle(),
            AbsActive = reader.ReadByte() != 0
        };
    }

    internal static PmrVehicleTelemetryConstant ReadPmrVehicleTelemetryConstant(this BinaryReader reader)
    {
        return new PmrVehicleTelemetryConstant
        {
            ChassisBoundingBoxMin = reader.ReadPmrVector3(),
            ChassisBoundingBoxMax = reader.ReadPmrVector3(),
            StarterIdleRpm = reader.ReadSingle(),
            EngineTorquePeakRpm = reader.ReadSingle(),
            EnginePowerPeakRpm = reader.ReadSingle(),
            EngineMaxRpm = reader.ReadSingle(),
            EngineMaxTorque = reader.ReadSingle(),
            EngineMaxPower = reader.ReadSingle(),
            EngineMaxBoost = reader.ReadSingle(),
            FuelCapacity = reader.ReadSingle(),
            BatteryCapacity = reader.ReadSingle(),
            TrackWidthFront = reader.ReadSingle(),
            TrackWidthRear = reader.ReadSingle(),
            Wheelbase = reader.ReadSingle(),
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
