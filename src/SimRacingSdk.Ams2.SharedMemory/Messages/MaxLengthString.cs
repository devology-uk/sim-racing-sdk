// See https://stackoverflow.com/questions/2266613/marshaling-a-c-two-dimensional-fixed-length-char-array-as-a-structure-member

using System.Runtime.InteropServices;

namespace SimRacingSdk.Ams2.SharedMemory.Messages;

public struct MaxLengthString
{
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = SharedMemoryConstants.MaxStringLength)]
    public string Value;
}