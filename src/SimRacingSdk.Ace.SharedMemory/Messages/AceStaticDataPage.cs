#nullable disable

using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using SimRacingSdk.Ace.SharedMemory.Enums;

namespace SimRacingSdk.Ace.SharedMemory.Messages;

// Field layout transcribed from ACE_SharedFileOut_Documentation_v1.pdf (SPageFileStaticEvo).
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Ansi)]
public class AceStaticDataPage
{
    private const string StaticMap = "Local\\acevo_pmf_static";

    private static readonly int size = Marshal.SizeOf<AceStaticDataPage>();
    private static readonly byte[] buffer = new byte[size];

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
    public string SharedMemoryVersion;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
    public string AceEvoVersion;
    public AceSessionType Session;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
    public string SessionName;
    public byte EventId;
    public byte SessionId;
    public AceStartingGrip StartingGrip;
    public float StartingAmbientTemperatureC;
    public float StartingGroundTemperatureC;
    [MarshalAs(UnmanagedType.I1)]
    public bool IsStaticWeather;
    [MarshalAs(UnmanagedType.I1)]
    public bool IsTimedRace;
    [MarshalAs(UnmanagedType.I1)]
    public bool IsOnline;
    public int NumberOfSessions;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
    public string Nation;
    public float Longitude;
    public float Latitude;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
    public string Track;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
    public string TrackConfiguration;
    public float TrackLengthM;

    public static AceStaticDataPage Read()
    {
        using var mappedFile = MemoryMappedFile.OpenExisting(StaticMap, MemoryMappedFileRights.Read);
        using var stream = mappedFile.CreateViewStream(0, 0, MemoryMappedFileAccess.Read);

        stream.ReadExactly(buffer, 0, buffer.Length);
        var handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
        var staticDataPage = Marshal.PtrToStructure<AceStaticDataPage>(handle.AddrOfPinnedObject());
        handle.Free();
        return staticDataPage;
    }
}
