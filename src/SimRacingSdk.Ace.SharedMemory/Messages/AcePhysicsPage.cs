#nullable disable

using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using SimRacingSdk.Ace.SharedMemory.Models;

namespace SimRacingSdk.Ace.SharedMemory.Messages;

// Field layout transcribed from ACE_SharedFileOut_Documentation_v1.pdf (SPageFilePhysics).
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Unicode)]
public class AcePhysicsPage
{
    private const string PhysicsMap = "Local\\acevo_pmf_physics";

    private static readonly int size = Marshal.SizeOf<AcePhysicsPage>();
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
    public float[] AccG;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public float[] WheelSlip;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public float[] WheelLoad;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public float[] WheelsPressure;
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
    public float Tc;
    public float Heading;
    public float Pitch;
    public float Roll;
    public float CgHeight;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
    public float[] CarDamage;
    public int NumberOfTyresOut;
    public int PitLimiterOn;
    public float Abs;
    public float KersCharge;
    public float KersInput;
    public int AutoShifterOn;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
    public float[] RideHeight;
    public float TurboBoost;
    public float Ballast;
    public float AirDensity;
    public float AirTemp;
    public float RoadTemp;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
    public float[] LocalAngularVelocity;
    public float FinalForceFeedback;
    public float PerformanceMeter;
    public int EngineBrake;
    public int ErsRecoveryLevel;
    public int ErsPowerLevel;
    public int ErsHeatCharging;
    public int ErsIsCharging;
    public float KersCurrentKiloJoules;
    public int DrsAvailable;
    public int DrsEnabled;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public float[] BrakeTemperature;
    public float Clutch;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public float[] TyreTempI;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public float[] TyreTempM;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public float[] TyreTempO;
    public int IsAiControlled;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public AceCoordinate3d[] TyreContactPoints;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public AceCoordinate3d[] TyreContactNormals;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public AceCoordinate3d[] TyreContactHeadings;
    public float BrakeBias;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
    public float[] LocalVelocity;
    public int P2PActivations;
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
    // Named BrakeTorque (Nm), not BrakePressure like Acc's equivalent field - the PDF documents this
    // as a genuinely different physical quantity in Evo, not just a rename.
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public float[] BrakeTorque;
    public int FrontBrakeCompound;
    public int RearBrakeCompound;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public float[] PadLife;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public float[] DiscLife;
    public int IgnitionOn;
    public int StarterEngineOn;
    public int IsEngineRunning;
    public float KerbVibration;
    public float SlipVibrations;
    public float RoadVibrations;
    public float AbsVibrations;

    public static AcePhysicsPage Read()
    {
        using var mappedFile = MemoryMappedFile.OpenExisting(PhysicsMap, MemoryMappedFileRights.Read);
        using var stream = mappedFile.CreateViewStream(0, 0, MemoryMappedFileAccess.Read);

        stream.ReadExactly(buffer, 0, buffer.Length);
        var handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
        var physicsPage = Marshal.PtrToStructure<AcePhysicsPage>(handle.AddrOfPinnedObject());
        handle.Free();
        return physicsPage;
    }
}
