// See https://stackoverflow.com/questions/2266613/marshaling-a-c-two-dimensional-fixed-length-char-array-as-a-structure-member

using System.Runtime.InteropServices;

namespace SimRacingSdk.Ams2.SharedMemory.Messages;

[StructLayout(LayoutKind.Sequential)]
public struct TyreCompoundName
{
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = SharedMemoryConstants.MaxTyreCompoundNameLength)]
    public string Value;
}