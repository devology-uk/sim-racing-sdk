#nullable disable

using System.Runtime.InteropServices;

namespace SimRacingSdk.Lmu.SharedMemory.Messages;

// Matches ScoringInfoV01 (InternalsPlugin.hpp, official LMU SDK header) exactly, including the ResultsStream/
// Vehicle pointer fields (IntPtr - 8 bytes on x64) purely to preserve byte-exact layout; both are process-local
// addresses meaningless to a reader in a different process, so they're never dereferenced here. The data they'd
// point to is read directly from LmuSharedMemoryScoringData's own flat arrays instead (see CopySharedMemoryObj in
// SharedMemoryInterface.hpp, which does the same thing on the writer side).
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Ansi)]
public struct LmuScoringInfo
{
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
    public string TrackName;
    public int Session;
    public double CurrentEt;
    public double EndEt;
    public int MaxLaps;
    public double LapDist;
    public IntPtr ResultsStream;

    public int NumVehicles;

    public byte GamePhase;
    public sbyte YellowFlagState;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
    public sbyte[] SectorFlag;
    public byte StartLight;
    public byte NumRedLights;
    [MarshalAs(UnmanagedType.I1)]
    public bool InRealtime;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
    public string PlayerName;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
    public string PlrFileName;

    // Weather
    public double DarkCloud;
    public double Raining;
    public double AmbientTemp;
    public double TrackTemp;
    public LmuVect3 Wind;
    public double MinPathWetness;
    public double MaxPathWetness;

    // Multiplayer
    public byte GameMode;
    [MarshalAs(UnmanagedType.I1)]
    public bool IsPasswordProtected;
    public ushort ServerPort;
    public uint ServerPublicIp;
    public int MaxPlayers;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
    public string ServerName;
    public float StartEt;

    public double AvgPathWetness;
    public float SessionTimeRemaining;
    public float TimeOfDay;
    [MarshalAs(UnmanagedType.I1)]
    public bool IsFixedSetup;
    public byte TrackGripLevel;
    public byte CloudCoverage;
    public byte TrackLimitsStepsPerPenalty;
    public byte TrackLimitsStepsPerPoint;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 187)]
    public byte[] Expansion;

    public IntPtr Vehicle;

    public override readonly string ToString()
    {
        return $"LmuScoringInfo {{ TrackName = {this.TrackName}, Session = {this.Session}, "
             + $"CurrentEt = {this.CurrentEt}, MaxLaps = {this.MaxLaps}, NumVehicles = {this.NumVehicles}, "
             + $"GamePhase = {this.GamePhase}, YellowFlagState = {this.YellowFlagState}, "
             + $"InRealtime = {this.InRealtime}, AmbientTemp = {this.AmbientTemp}, TrackTemp = {this.TrackTemp}, "
             + $"TrackGripLevel = {this.TrackGripLevel} }}";
    }
}
