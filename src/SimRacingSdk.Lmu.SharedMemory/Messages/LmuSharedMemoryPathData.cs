#nullable disable

using System.Runtime.InteropServices;

namespace SimRacingSdk.Lmu.SharedMemory.Messages;

// Matches SharedMemoryPathData (SharedMemoryInterface.hpp) - the game's own reported paths, only populated on
// Enter/Exit/SetEnvironment events. A more reliable alternative to LmuPathProvider's Steam-based guessing once the
// game is actually running, though only available at that point - LmuPathProvider still owns pre-launch resolution.
//
// Pack = 8, not 4: declared outside InternalsPlugin.hpp's pack(4) region - see LmuSharedMemoryObjectOut.
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 8, CharSet = CharSet.Ansi)]
public struct LmuSharedMemoryPathData
{
    private const int MaxPath = 260;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MaxPath)]
    public string UserData;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MaxPath)]
    public string CustomVariables;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MaxPath)]
    public string StewardResults;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MaxPath)]
    public string PlayerProfile;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MaxPath)]
    public string PluginsFolder;
}
