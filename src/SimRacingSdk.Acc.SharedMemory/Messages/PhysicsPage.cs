#nullable disable

using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using SimRacingSdk.Acc.SharedMemory.Models;

namespace SimRacingSdk.Acc.SharedMemory.Messages;

[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Unicode)]
public class PhysicsPage
{
    private const string PhysicsMap = "Local\\acpmf_physics";

    private static readonly int size = Marshal.SizeOf<PhysicsPage>();
    private static readonly byte[] buffer = new byte[size];

    public int PacketId;
    public float Gas;
    public float Brake;
    public float Fuel;
    public int Gear;
    public int Rpm;
    public float SteerAngle;
    public float SpeedKmh;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
    public float[] Velocity;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
    public float[] AccelerationVector;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public float[] WheelSlip;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public float[] WheelLoad;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public float[] WheelPressure;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public float[] WheelAngularSpeed;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public float[] TyreWear;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public float[] TyreDirtyLevel;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public float[] TyreCoreTemperature;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public float[] CamberRad;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public float[] SuspensionTravel;
    public float Drs;
    public float TC;
    public float Heading;
    public float Pitch;
    public float Roll;
    public float CentreOfGravityHeight;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
    public float[] CarDamage;
    public int NumberOfTyresOut;
    [MarshalAs(UnmanagedType.Bool)]
    public bool PitLimiterOn;
    public float Abs;
    public float KersCharge;
    public float KersInput;
    [MarshalAs(UnmanagedType.Bool)]
    public bool AutoShifterOn;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
    public float[] RideHeight;
    public float TurboBoost;
    public float Ballast;
    public float AirDensity;
    public float AirTemp;
    public float RoadTemp;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
    public float[] LocalAngularVelocity;
    public float ForceFeedbackSignal;
    public float PerformanceMeter;
    public int EngineBrake;
    public int ErsRecoveryLevel;
    public int ErsPowerLevel;
    public int ErsHeatCharging;
    public int ErsIsCharging;
    public float KersCurrentKiloJoules;
    [MarshalAs(UnmanagedType.Bool)]
    public bool DrsAvailable;
    [MarshalAs(UnmanagedType.Bool)]
    public bool DrsEnabled;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public float[] BrakeTemperature;
    public float Clutch;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public float[] TyreTempI;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public float[] TyreTempM;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public float[] TyreTempO;
    [MarshalAs(UnmanagedType.Bool)]
    public bool IsAiControlled;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public AccCoordinate3d[] TyreContactPoints;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public AccCoordinate3d[] TyreContactNormals;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public AccCoordinate3d[] TyreContactHeadings;
    public float BrakeBias;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
    public float[] LocalVelocity;
    public int P2PActivation;
    public int P2PStatus;
    public int CurrentMaxRpm;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public float[] Mz;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public float[] Fx;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public float[] Fy;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public float[] SlipRatio;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public float[] SlipAngle;
    public int TcInAction;
    public int AbsInAction;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public float[] SuspensionDamage;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public float[] TyreTemp;
    public float WaterTemp;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public float[] BrakePressure;
    public int FrontBrakeCompound;
    public int RearBrakeCompound;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public float[] PadLife;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public float[] DiscLife;
    [MarshalAs(UnmanagedType.Bool)]
    public bool IgnitionOn;
    [MarshalAs(UnmanagedType.Bool)]
    public bool StarterEngineOn;
    [MarshalAs(UnmanagedType.Bool)]
    public bool IsEngineRunning;
    public float KerbVibrations;
    public float SlipVibrations;
    public float GearVibrations;
    public float AbsVibrations;
   

    public static PhysicsPage Read()
    {
        using var mappedFile = MemoryMappedFile.OpenExisting(PhysicsMap, MemoryMappedFileRights.Read);
        using var stream = mappedFile.CreateViewStream(0, 0, MemoryMappedFileAccess.Read);

        stream.ReadExactly(buffer, 0, buffer.Length);
        var handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
        var physicsPage = Marshal.PtrToStructure<PhysicsPage>(handle.AddrOfPinnedObject());
        handle.Free();
        return physicsPage;
    }
}