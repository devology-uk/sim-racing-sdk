#nullable disable

using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;

namespace SimRacingSdk.Acc.SharedMemory.Messages;

[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Unicode)]
public class StaticDataPage
{
    private const string StaticMap = "Local\\acpmf_static";

    private static readonly int size = Marshal.SizeOf<StaticDataPage>();
    private static readonly byte[] buffer = new byte[size];
    
    private static StaticDataPage cachedPage;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
    public string SharedMemoryVersion;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
    public string AccVersion;
    public int NumberOfSessions;
    public int NumberOfCars;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
    public string CarModel;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
    public string TrackName;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
    public string PlayerFirstName;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
    public string PlayerSurname;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
    public string PlayerNickname;
    public int SectorCount;
    public float MaxTorque;
    public float MaxPower;
    public int MaxRpm;
    public float MaxFuel;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public float[] SuspensionMaxTravel;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public float[] TyreRadius;
    public float MaxTurboBoost;
    public float AirTemperature;
    public float RoadTemperature;
    [MarshalAs(UnmanagedType.Bool)]
    public bool PenaltiesEnabled;
    public float AidFuelRate;
    public float AidTireRate;
    public float AidMechanicalDamage;
    [MarshalAs(UnmanagedType.Bool)]
    public bool AidAllowTyreBlankets;
    public float AidStability;
    [MarshalAs(UnmanagedType.Bool)]
    public bool AidAutoClutch;
    [MarshalAs(UnmanagedType.Bool)]
    public bool AidAutoBlip;
    [MarshalAs(UnmanagedType.Bool)]
    public bool HasDrs;
    [MarshalAs(UnmanagedType.Bool)]
    public bool HasErs;
    [MarshalAs(UnmanagedType.Bool)]
    public bool HasKers;
    public float KersMaxJoules;
    public int EngineBrakeSettingsCount;
    public int ErsPowerControllerCount;
    public float TrackSplineLength;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
    public string TrackConfiguration;
    public float ErsMaxJoules;
    [MarshalAs(UnmanagedType.Bool)]
    public bool IsTimedRace;
    [MarshalAs(UnmanagedType.Bool)]
    public bool HasExtraLap;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
    public string CarSkin;
    public int ReversedGridPositions;
    public int PitWindowStart;
    public int PitWindowEnd;
    [MarshalAs(UnmanagedType.Bool)]
    public bool IsOnline;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
    public string DryTyresName;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
    public string WetTyresName;

    public static bool TryRead(out StaticDataPage staticDataPage)
    {
        if (cachedPage != null)
        {
            staticDataPage = cachedPage;
            return true;
        }

        try
        {
            using var mappedFile = MemoryMappedFile.OpenExisting(StaticMap, MemoryMappedFileRights.Read);
            using var stream = mappedFile.CreateViewStream();
            stream.ReadExactly(buffer, 0, buffer.Length);
            var handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            cachedPage = Marshal.PtrToStructure<StaticDataPage>(handle.AddrOfPinnedObject());
            handle.Free();
            staticDataPage = cachedPage;
            return true;
        }
        catch(FileNotFoundException)
        {
            staticDataPage = null;
            return false;
        }
    }
}