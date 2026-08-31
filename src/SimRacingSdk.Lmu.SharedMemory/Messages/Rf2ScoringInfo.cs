#nullable disable

using System.Runtime.InteropServices;

namespace SimRacingSdk.Lmu.SharedMemory.Messages;

// Matches rF2ScoringInfo (rF2State.h) / ScoringInfoV01 exactly, including the pointer1/pointer2 8-byte (x64)
// placeholders the plugin substitutes for pointers it can't expose - kept as unused byte arrays purely to preserve
// byte-exact layout.
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Ansi)]
public struct Rf2ScoringInfo
{
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
    public string TrackName;
    public int Session;
    public double CurrentEt;
    public double EndEt;
    public int MaxLaps;
    public double LapDist;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
    public byte[] Pointer1;

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
    public Rf2Vec3 Wind;
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

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 200)]
    public byte[] Expansion;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
    public byte[] Pointer2;

    public override readonly string ToString()
    {
        return $"Rf2ScoringInfo {{ TrackName = {this.TrackName}, Session = {this.Session}, "
             + $"CurrentEt = {this.CurrentEt}, MaxLaps = {this.MaxLaps}, NumVehicles = {this.NumVehicles}, "
             + $"GamePhase = {this.GamePhase}, YellowFlagState = {this.YellowFlagState}, "
             + $"InRealtime = {this.InRealtime}, AmbientTemp = {this.AmbientTemp}, TrackTemp = {this.TrackTemp} }}";
    }
}
