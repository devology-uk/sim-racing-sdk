#nullable disable

using System.Runtime.InteropServices;

namespace SimRacingSdk.Lmu.SharedMemory.Messages;

// Matches ApplicationStateV01 (InternalsPlugin.hpp, official LMU SDK header).
[Serializable]
[StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Ansi)]
public struct LmuApplicationState
{
    public IntPtr AppWindow;
    public uint Width;
    public uint Height;
    public uint RefreshRate;
    public uint Windowed;
    public byte OptionsLocation;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
    public string OptionsPage;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 204)]
    public byte[] Expansion;

    public override readonly string ToString()
    {
        return $"LmuApplicationState {{ Width = {this.Width}, Height = {this.Height}, "
             + $"RefreshRate = {this.RefreshRate}, Windowed = {this.Windowed} }}";
    }
}
