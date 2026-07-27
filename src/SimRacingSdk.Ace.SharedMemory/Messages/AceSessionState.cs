#nullable disable

using System.Runtime.InteropServices;

namespace SimRacingSdk.Ace.SharedMemory.Messages;

// SMEvoSessionState from the PDF, fixed at 256 bytes. See AceTyreState.cs for why a trailing
// Reserved array is used to hit that documented size - unverified against a live game.
// Embedded inside AceGraphicsPage - not to be confused with Models.AceSharedMemorySession.
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Ansi)]
public struct AceSessionState
{
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
    public string PhaseName;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
    public string TimeLeft;
    public int TimeLeftMs;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
    public string WaitTime;
    public int TotalLap;
    public int CurrentLap;
    public int LightsOn;
    public int LightsMode;
    public float LapLengthKm;
    public int EndSessionFlag;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
    public string TimeToNextSession;
    [MarshalAs(UnmanagedType.I1)]
    public bool DisconnectedFromServer;
    [MarshalAs(UnmanagedType.I1)]
    public bool RestartSeasonEnabled;
    [MarshalAs(UnmanagedType.I1)]
    public bool UiEnableDrive;
    [MarshalAs(UnmanagedType.I1)]
    public bool UiEnableSetup;
    [MarshalAs(UnmanagedType.I1)]
    public bool IsReadyToNextBlinking;
    [MarshalAs(UnmanagedType.I1)]
    public bool ShowWaitingForPlayers;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 140)]
    public byte[] Reserved;
}
