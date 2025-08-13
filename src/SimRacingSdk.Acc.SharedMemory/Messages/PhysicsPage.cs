#nullable disable

using System.IO.MemoryMappedFiles;
using SimRacingSdk.Acc.SharedMemory.Models;

namespace SimRacingSdk.Acc.SharedMemory.Messages;

public class PhysicsPage
{
    private const string PhysicsMap = "Local\\acpmf_physics";

    public float Abs;
    public float AbsVibrations;
    public float[] AccG;
    public float AirTemp;
    public bool AutoShifterOn;
    public float Brake;
    public float BrakeBias;
    public float[] BrakePressure;
    public float[] BrakeTemperature;
    public float[] CarDamage;
    public float Clutch;
    public float[] DiscLife;
    public float FinalFF;
    public int FrontBrakeCompound;
    public float Fuel;
    public float Gas;
    public int Gear;
    public float GVibrations;
    public float Heading;
    public bool IgnitionOn;
    public bool IsAiControlled;
    public bool IsEngineRunning;
    public float KerbVibration;
    public float[] LocalAngularVelocity;
    public float[] LocalVelocity;
    public int PacketId;
    public float[] PadLife;
    public float Pitch;
    public bool PitLimiterOn;
    public int RearBrakeCompound;
    public float RoadTemp;
    public float Roll;
    public int Rpm;
    public float[] SlipAngle;
    public float[] SlipRatio;
    public float SlipVibrations;
    public float SpeedKmh;
    public bool StarterEngineOn;
    public float SteerAngle;
    public float[] SuspensionDamage;
    public float[] SuspensionTravel;
    public float TC;
    public float TurboBoost;
    public AccRtVector3d[] TyreContactHeading;
    public AccRtVector3d[] TyreContactNormal;
    public AccRtVector3d[] TyreContactPoint;
    public float[] TyreCoreTemperature;
    public float[] TyreTemp;
    public float[] Velocity;
    public float WaterTemp;
    public float[] WheelAngularSpeed;
    public float[] WheelPressure;
    public float[] WheelSlip;

    public static bool TryRead(out PhysicsPage physicsPage)
    {
        try
        {
            using var mappedFile = MemoryMappedFile.OpenExisting(PhysicsMap, MemoryMappedFileRights.Read);
            using var stream = mappedFile.CreateViewStream();
            var reader = new BinaryReader(stream);
            physicsPage = new PhysicsPage
            {
                PacketId = reader.ReadInt32(),
                Gas = reader.ReadSingle(),
                Brake = reader.ReadSingle(),
                Fuel = reader.ReadSingle(),
                Gear = reader.ReadInt32(),
                Rpm = reader.ReadInt32(),
                SteerAngle = reader.ReadSingle(),
                SpeedKmh = reader.ReadSingle(),
                Velocity =
                [
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle()
                ],
                AccG =
                [
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle()
                ],
                WheelSlip =
                [
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle()
                ],
                WheelLoad =
                [
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle()
                ],
                WheelPressure =
                [
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle()
                ],
                WheelAngularSpeed =
                [
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle()
                ],
                TyreWear =
                [
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle()
                ],
                TyreDirtyLevel =
                [
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle()
                ],
                TyreCoreTemperature =
                [
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle()
                ],
                CamberRad =
                [
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle()
                ],
                SuspensionTravel =
                [
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle()
                ],
                Drs = reader.ReadSingle(),
                TC = reader.ReadSingle(),
                Heading = reader.ReadSingle(),
                Pitch = reader.ReadSingle(),
                Roll = reader.ReadSingle(),
                CgHeight = reader.ReadSingle(),
                CarDamage =
                [
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle()
                ],
                NumberOfTyresOut = reader.ReadInt32(),
                PitLimiterOn = reader.ReadBoolean(),
                Abs = reader.ReadSingle(),
                KersCharge = reader.ReadSingle(),
                KersInput = reader.ReadSingle(),
                AutoShifterOn = reader.ReadBoolean(),
                RideHeight =
                [
                    reader.ReadSingle(),
                    reader.ReadSingle()
                ],
                TurboBoost = reader.ReadSingle(),
                Ballast = reader.ReadSingle(),
                AirDensity = reader.ReadSingle(),
                AirTemp = reader.ReadSingle(),
                RoadTemp = reader.ReadSingle(),
                LocalAngularVelocity =
                [
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle()
                ],
                FinalFF = reader.ReadSingle(),
                PerformanceMeter = reader.ReadSingle(),
                EngineBrake = reader.ReadInt32(),
                ErsRecoveryLevel = reader.ReadInt32(),
                ErsPowerLevel = reader.ReadInt32(),
                ErsHeatCharging = reader.ReadInt32(),
                ErsIsCharging = reader.ReadInt32(),
                KersCurrentKJ = reader.ReadSingle(),
                DrsAvailable = reader.ReadBoolean(),
                DrsEnabled = reader.ReadBoolean(),
                BrakeTemperature =
                [
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle()
                ],
                Clutch = reader.ReadSingle(),
                TyreTempI =
                [
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle()
                ],
                TyreTempM =
                [
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle()
                ],
                TyreTempO =
                [
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle()
                ],
                IsAiControlled = reader.ReadBoolean(),
                TyreContactPoint =
                [
                    new AccRtVector3d
                    {
                        X = reader.ReadSingle(),
                        Y = reader.ReadSingle(),
                        Z = reader.ReadSingle()
                    },
                    new AccRtVector3d
                    {
                        X = reader.ReadSingle(),
                        Y = reader.ReadSingle(),
                        Z = reader.ReadSingle()
                    },
                    new AccRtVector3d
                    {
                        X = reader.ReadSingle(),
                        Y = reader.ReadSingle(),
                        Z = reader.ReadSingle()
                    },
                    new AccRtVector3d
                    {
                        X = reader.ReadSingle(),
                        Y = reader.ReadSingle(),
                        Z = reader.ReadSingle()
                    }
                ],
                TyreContactNormal =
                [
                    new AccRtVector3d
                    {
                        X = reader.ReadSingle(),
                        Y = reader.ReadSingle(),
                        Z = reader.ReadSingle()
                    },
                    new AccRtVector3d
                    {
                        X = reader.ReadSingle(),
                        Y = reader.ReadSingle(),
                        Z = reader.ReadSingle()
                    },
                    new AccRtVector3d
                    {
                        X = reader.ReadSingle(),
                        Y = reader.ReadSingle(),
                        Z = reader.ReadSingle()
                    },
                    new AccRtVector3d
                    {
                        X = reader.ReadSingle(),
                        Y = reader.ReadSingle(),
                        Z = reader.ReadSingle()
                    }
                ],
                TyreContactHeading =
                [
                    new AccRtVector3d
                    {
                        X = reader.ReadSingle(),
                        Y = reader.ReadSingle(),
                        Z = reader.ReadSingle()
                    },
                    new AccRtVector3d
                    {
                        X = reader.ReadSingle(),
                        Y = reader.ReadSingle(),
                        Z = reader.ReadSingle()
                    },
                    new AccRtVector3d
                    {
                        X = reader.ReadSingle(),
                        Y = reader.ReadSingle(),
                        Z = reader.ReadSingle()
                    },
                    new AccRtVector3d
                    {
                        X = reader.ReadSingle(),
                        Y = reader.ReadSingle(),
                        Z = reader.ReadSingle()
                    }
                ],
                BrakeBias = reader.ReadSingle(),
                LocalVelocity =
                [
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle()
                ],
                P2PActivations = reader.ReadInt32(),
                P2PStatus = reader.ReadInt32(),
                CurrentMaxRpm = reader.ReadInt32(),
                Mz =
                [
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle()
                ],
                Fx =
                [
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle()
                ],
                Fy =
                [
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle()
                ],
                SlipRatio =
                [
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle()
                ],
                SlipAngle =
                [
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle()
                ],
                TcInAction = reader.ReadInt32(),
                AbsInAction = reader.ReadInt32(),
                SuspensionDamage =
                [
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle()
                ],
                TyreTemp =
                [
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle()
                ],
                WaterTemp = reader.ReadSingle(),
                BrakePressure =
                [
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle()
                ],
                FrontBrakeCompound = reader.ReadInt32(),
                RearBrakeCompound = reader.ReadInt32(),
                PadLife =
                [
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle()
                ],
                DiscLife =
                [
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle(),
                    reader.ReadSingle()
                ],
                IgnitionOn = reader.ReadBoolean(),
                StarterEngineOn = reader.ReadBoolean(),
                IsEngineRunning = reader.ReadBoolean(),
                KerbVibration = reader.ReadSingle(),
                SlipVibrations = reader.ReadSingle(),
                GVibrations = reader.ReadSingle(),
                AbsVibrations = reader.ReadSingle()
            };

            return true;
        }
        catch(FileNotFoundException)
        {
            physicsPage = null;
            return false;
        }
    }

    #region Not populated by ACC

    public float[] WheelLoad;
    public float[] TyreWear;
    public float[] TyreDirtyLevel;
    public float[] CamberRad;
    public float Drs;
    public float CgHeight;
    public int NumberOfTyresOut;
    public float KersCharge;
    public float KersInput;
    public float[] RideHeight;
    public float Ballast;
    public float AirDensity;
    public float PerformanceMeter;
    public int EngineBrake;
    public int ErsRecoveryLevel;
    public int ErsPowerLevel;
    public int ErsHeatCharging;
    public int ErsIsCharging;
    public float KersCurrentKJ;
    public bool DrsAvailable;
    public bool DrsEnabled;
    public float[] TyreTempI;
    public float[] TyreTempM;
    public float[] TyreTempO;
    public int P2PActivations;
    public int P2PStatus;
    public int CurrentMaxRpm;
    public float[] Mz;
    public float[] Fx;
    public float[] Fy;
    public int TcInAction;
    public int AbsInAction;

    #endregion
}