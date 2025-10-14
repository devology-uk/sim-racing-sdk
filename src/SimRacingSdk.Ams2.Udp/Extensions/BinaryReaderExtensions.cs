using SimRacingSdk.Ams2.Udp.Enums;
using SimRacingSdk.Ams2.Udp.Messages;
using SimRacingSdk.Core;

namespace SimRacingSdk.Ams2.Udp.Extensions;

internal static class BinaryReaderExtensions
{
    internal static byte[] ReadByteArray(this BinaryReader reader, int size)
    {
        var result = new byte[size];

        for(var i = 0; i < size; i++)
        {
            result[i] = reader.ReadByte();
        }

        return result;
    }

    internal static GameStateUpdate ReadGameStateUpdate(this BinaryReader reader, MessageHeader header)
    {
        var result = new GameStateUpdate(header)
        {
            BuildVersionNumber = reader.ReadInt16()
        };

        var state = reader.ReadByte();
        result.SessionState = state.TopFourBits();
        result.GameState = state.BottomFourBits();
        result.AmbientTemperature = reader.ReadByte();
        result.TrackTemperature = reader.ReadByte();
        result.RainDensity = reader.ReadByte();
        result.SnowDensity = reader.ReadByte();
        result.WindSpeed = reader.ReadByte();
        result.WindDirectionX = reader.ReadByte();
        result.WindDirectionY = reader.ReadByte();

        return result;
    }

    internal static MessageHeader ReadHeader(this BinaryReader reader)
    {
        return new MessageHeader
        {
            PacketNumber = reader.ReadInt32(),
            CategoryPacketNumber = reader.ReadInt32(),
            PartialPacketIndex = reader.ReadByte(),
            PartialPacketNumber = reader.ReadByte(),
            PacketType = (InboundMessageType)reader.ReadByte(),
            PacketVersion = reader.ReadByte(),
            PacketLength = reader.BaseStream.Length
        };
    }

    internal static short[] ReadInt16Array(this BinaryReader reader, int size)
    {
        var result = new short[size];

        for(var i = 0; i < size; i++)
        {
            result[i] = reader.ReadInt16();
        }

        return result;
    }

    internal static int[] ReadInt32Array(this BinaryReader reader, int size)
    {
        var result = new int[size];

        for(var i = 0; i < size; i++)
        {
            result[i] = reader.ReadInt32();
        }

        return result;
    }

    internal static ParticipantsUpdate ReadParticipantsUpdate(this BinaryReader reader, MessageHeader header)
    {
        const int ParticipantsPerUpdate = 16;

        var result = new ParticipantsUpdate(header)
        {
            ChangeTimestamp = reader.ReadInt32(),
            Names = reader.ReadStringArray(ParticipantsPerUpdate),
            Nationalities = reader.ReadInt32Array(ParticipantsPerUpdate),
            Indexes = reader.ReadInt16Array(ParticipantsPerUpdate)
        };

        return result;
    }

    internal static RaceInfoUpdate ReadRaceInfoUpdate(this BinaryReader reader, MessageHeader header)
    {
        var result = new RaceInfoUpdate(header)
        {
            OverallBestLapTime = reader.ReadInt32(),
            PersonalBestLapTime = reader.ReadInt32(),
            PersonalBestSector1 = reader.ReadInt32(),
            PersonalBestSector2 = reader.ReadInt32(),
            PersonalBestSector3 = reader.ReadInt32(),
            OverallBestSector1 = reader.ReadInt32(),
            OverallBestSector2 = reader.ReadInt32(),
            OverallBestSector3 = reader.ReadInt32(),
            TrackLength = reader.ReadInt32(),
            TrackLocation = reader.ReadString(),
            TrackLayout = reader.ReadString(),
            TranslatedTrackLocation = reader.ReadString(),
            TranslatedTrackLayout = reader.ReadString()
        };

        var scheduleInfo = reader.ReadInt16();
        var isTimedEvent = scheduleInfo < 0;
        result.IsTimedEvent = isTimedEvent;
        result.ScheduledLaps = isTimedEvent? 0: scheduleInfo;
        var scheduledTime = isTimedEvent? ((ushort)scheduleInfo & 32767) * 5: 0;
        result.ScheduledTime = TimeSpan.FromSeconds(scheduledTime);

        result.IsPitStopMandatory = reader.ReadByte();

        return result;
    }

    internal static string[] ReadStringArray(this BinaryReader reader, int size)
    {
        var result = new string[size];

        for(var i = 0; i < size; i++)
        {
            result[i] = reader.ReadString();
        }

        return result;
    }

    internal static TelemetryUpdate ReadTelemetryUpdate(this BinaryReader reader, MessageHeader header)
    {
        var result = new TelemetryUpdate(header)
        {
            ParticipantIndex = reader.ReadByte(),
            UnfilteredThrottle = reader.ReadByte(),
            UnfilteredBrake = reader.ReadByte(),
            UnfilteredSteering = reader.ReadByte(),
            UnfilteredClutch = reader.ReadByte(),
            CarFlags = reader.ReadByte(),
            OilTempC = reader.ReadInt16(),
            OilPressureKPa = reader.ReadInt16(),
            WaterTempC = reader.ReadInt16(),
            WaterPressureKPa = reader.ReadInt16(),
            FuelPressureKPa = reader.ReadInt16(),
            FuelCapacity = reader.ReadByte(),
            Brake = reader.ReadByte(),
            Throttle = reader.ReadByte(),
            Clutch = reader.ReadByte(),
            FuelLevel = reader.ReadInt32(),
            Speed = reader.ReadInt32(),
            Rpm = reader.ReadInt16(),
            Steering = reader.ReadByte()
        };

        var gearInfo = reader.ReadByte();
        result.AvailableGears = gearInfo.TopFourBits();
        result.Gear = gearInfo.BottomFourBits();

        result.BoostAmount = reader.ReadByte();
        result.CrashState = reader.ReadByte();
        result.OdometerKmh = reader.ReadInt32();
        result.Orientation = reader.ReadInt32Array(3);
        result.LocalVelocity = reader.ReadInt32Array(3);
        result.WorldVelocity = reader.ReadInt32Array(3);
        result.AngularVelocity = reader.ReadInt32Array(3);
        result.LocalAcceleration = reader.ReadInt32Array(3);
        result.WorldAcceleration = reader.ReadInt32Array(3);
        result.ExtentsCentre = reader.ReadInt32Array(3);
        result.TyreFlags = reader.ReadByteArray(4);
        result.Terrain = reader.ReadByteArray(4);
        result.TyreY = reader.ReadInt32Array(4);
        result.TyreRps = reader.ReadInt32Array(4);
        result.TyreTemp = reader.ReadByteArray(4);
        result.TyreHeightAboveGround = reader.ReadInt32Array(4);
        result.TyreWear = reader.ReadByteArray(4);
        result.BrakeDamage = reader.ReadByteArray(4);
        result.SuspensionDamage = reader.ReadByteArray(4);
        result.BrakeTempC = reader.ReadInt16Array(4);
        result.TyreTreadTempC = reader.ReadInt16Array(4);
        result.TyreLayerTempC = reader.ReadInt16Array(4);
        result.TyreCarcassTempC = reader.ReadInt16Array(4);
        result.TyreRimTempC = reader.ReadInt16Array(4);
        result.TyreInternalAirTempC = reader.ReadInt16Array(4);
        result.TyreTempLeftC = reader.ReadInt16Array(4);
        result.TyreTempCentreC = reader.ReadInt16Array(4);
        result.TyreTempRightC = reader.ReadInt16Array(4);
        result.WheelLocalPositionY = reader.ReadInt32Array(4);
        result.RideHeight = reader.ReadInt32Array(4);
        result.SuspensionTravel = reader.ReadInt32Array(4);
        result.SuspensionVelocity = reader.ReadInt32Array(4);
        result.SuspensionRideHeight = reader.ReadInt16Array(4);
        result.AirPressure = reader.ReadInt16Array(4);
        result.EngineSpeed = reader.ReadInt32();
        result.EngineTorque = reader.ReadInt32();
        result.Wings = reader.ReadByteArray(2);
        result.Handbrake = reader.ReadByte();
        result.AeroDamage = reader.ReadByte();
        result.EngineDamage = reader.ReadByte();
        result.JoyPad0 = reader.ReadInt32();
        result.DPad = reader.ReadByte();
        result.TyreCompound = reader.ReadString();
        result.TurboBoostPressure = reader.ReadInt32();
        result.FullPosition = reader.ReadInt32Array(3);
        result.BrakeBias = reader.ReadByte();
        result.TickCount = reader.ReadInt32();

        return result;
    }

    internal static TimeStatsUpdate ReadTimeStatsUpdate(this BinaryReader reader, MessageHeader header)
    {
        var result = new TimeStatsUpdate(header)
        {
            ChangeTimestamp = reader.ReadInt32(),
            ParticipantStats = reader.ReadParticipantStats()
        };

        return result;
    }

    internal static TimingsUpdate ReadTimingsUpdate(this BinaryReader reader, MessageHeader header)
    {
        var result = new TimingsUpdate(header)
        {
            ParticipantCount = reader.ReadByte(),
            ChangeTimeStamp = reader.ReadInt32(),
            EventTimeRemaining = reader.ReadInt32(),
            SplitTimeAhead = reader.ReadInt32(),
            SplitTimeBehind = reader.ReadInt32(),
            SplitTime = reader.ReadInt32(),
            Participants = reader.ReadParticipantsInfo(),
            LocalParticipantIndex = reader.ReadInt16(),
            TickCount = reader.ReadInt32()
        };

        return result;
    }

    internal static VehicleClassUpdate ReadVehicleClassUpdate(this BinaryReader reader, MessageHeader header)
    {
        var result = new VehicleClassUpdate(header)
        {
            Classes = reader.ReadVehicleClasses()
        };

        return result;
    }

    internal static VehicleInfoUpdate ReadVehicleInfoUpdate(this BinaryReader reader, MessageHeader header)
    {
        var result = new VehicleInfoUpdate(header)
        {
            Vehicles = reader.ReadVehicles()
        };

        return result;
    }

    private static ParticipantInfo[] ReadParticipantsInfo(this BinaryReader reader)
    {
        const int ParticipantsPerUpdate = 32;

        var result = new ParticipantInfo[ParticipantsPerUpdate];

        for(var i = 0; i < ParticipantsPerUpdate; i++)
        {
            var participantInfo = new ParticipantInfo
            {
                WorldPosition = reader.ReadInt16Array(3),
                Orientation = reader.ReadInt16Array(3),
                CurrentLapDistance = reader.ReadInt16()
            };

            var positionInfo = reader.ReadByte();
            participantInfo.IsActive = positionInfo.TopBit() != 0;
            participantInfo.Position = positionInfo.BottomSevenBits();

            var sectorInfo = reader.ReadByte();
            participantInfo.ZExtraPrecision = sectorInfo.TopTwoBits();
            participantInfo.XExtraPrecision = sectorInfo.SecondTwoBits();
            participantInfo.Sector = sectorInfo.BottomFourBits();

            var flagInfo = reader.ReadByte();
            participantInfo.FlagReason = flagInfo.TopFourBits();
            participantInfo.FlagColour = flagInfo.BottomFourBits();

            var pitModeInfo = reader.ReadByte();
            participantInfo.PitSchedule = pitModeInfo.TopFourBits();
            participantInfo.PitMode = pitModeInfo.BottomFourBits();

            participantInfo.CarIndex = reader.ReadInt16();

            var raceStateInfo = reader.ReadByte();
            participantInfo.RaceStateFlags = raceStateInfo.BottomThreeBits();
            participantInfo.IsLapInvalid = raceStateInfo.FourthBit() != 0;

            participantInfo.CurrentLap = reader.ReadByte();
            participantInfo.CurrentTime = reader.ReadInt32();
            participantInfo.ParticipantIndex = reader.ReadInt16();

            result[i] = participantInfo;
        }

        return result;
    }

    private static ParticipantStats[] ReadParticipantStats(this BinaryReader reader)
    {
        const int ParticipantsPerUpdate = 32;

        var result = new ParticipantStats[ParticipantsPerUpdate];

        for(var i = 0; i < ParticipantsPerUpdate; i++)
        {
            var participantStats = new ParticipantStats
            {
                BestLapTime = reader.ReadInt32(),
                LastLapTime = reader.ReadInt32(),
                LastSectorTime = reader.ReadInt32(),
                BestSector1 = reader.ReadInt32(),
                BestSector2 = reader.ReadInt32(),
                BestSector3 = reader.ReadInt32(),
                OnlineRep = reader.ReadInt32(),
                ParticipantIndex = reader.ReadInt16()
            };

            result[i] = participantStats;
        }

        return result;
    }

    private static VehicleClassInfo[] ReadVehicleClasses(this BinaryReader reader)
    {
        const int VehicleClassesPerUpdate = 60;

        var result = new VehicleClassInfo[VehicleClassesPerUpdate];

        for(var i = 0; i < VehicleClassesPerUpdate; i++)
        {
            var vehicleClassInfo = new VehicleClassInfo()
            {
                Index = reader.ReadInt32(),
                Name = reader.ReadString()
            };
        }

        return result;
    }

    private static VehicleInfo[] ReadVehicles(this BinaryReader reader)
    {
        const int VehiclesPerUpdate = 16;

        var result = new VehicleInfo[VehiclesPerUpdate];

        for(var i = 0; i < VehiclesPerUpdate; i++)
        {
            var vehicleInfo = new VehicleInfo()
            {
                Index = reader.ReadInt32(),
                Class = reader.ReadInt32(),
                Name = reader.ReadString()
            };

            result[i] = vehicleInfo;
        }

        return result;
    }
}